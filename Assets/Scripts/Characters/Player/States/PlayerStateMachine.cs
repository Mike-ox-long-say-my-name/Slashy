using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Modules;
using Core.Player;
using Core.Player.Interfaces;
using Core.Utilities;
using Effects;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerConfig = Configs.Player.PlayerConfig;

namespace Characters.Player.States
{
    [RequireComponent(typeof(MixinVelocityMovement))]
    [RequireComponent(typeof(MixinStamina))]
    [RequireComponent(typeof(MixinCharacter))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(MixinJumpHandler))]
    [RequireComponent(typeof(MixinHittable))]
    [RequireComponent(typeof(MixinPlayerCapabilities))]
    [RequireComponent(typeof(MixinAttackExecutorHelper))]
    [RequireComponent(typeof(MixinInteractor))]
    public class PlayerStateMachine : MonoBehaviour, IPlayer
    {
        public PlayerBaseState CurrentState { get; set; }

        [Space]
        [Header("Attacks")]
        [SerializeField] private MonoAbstractAttackExecutor lightAttackFirst;
        [SerializeField] private MonoAbstractAttackExecutor lightAttackSecond;
        [SerializeField] private MonoAbstractAttackExecutor lightAirboneAttack;
        [SerializeField] private MonoAbstractAttackExecutor firstStrongAttack;
        [SerializeField] private MonoAbstractAttackExecutor secondStrongAttack;

        [Space]
        [SerializeField] private PlayerConfig playerConfig;

        public bool IsInvincible { get; set; }
        public bool IsJumping => CurrentState.GetType() == typeof(PlayerJumpState);
        public bool IsDashing => CurrentState.GetType() == typeof(PlayerDashState);
        public bool IsFalling => CurrentState.GetType() == typeof(PlayerFallState);
        public bool IsStaggered => CurrentState.GetType() == typeof(PlayerAirboneStaggerState)
                                            || CurrentState.GetType() == typeof(PlayerGroundStaggerState);

        public bool IsAttacking { get; set; }

        private IPlayerCharacter _player;

        public IPlayerCharacter Player
        {
            get
            {
                if (_player != null)
                {
                    return _player;
                }

                var character = GetComponent<MixinCharacter>().Character;
                var stamina = GetComponent<MixinStamina>().Stamina;
                _player = new PlayerCharacter(character, stamina);
                return _player;
            }
        }

        public OwningLock DashRecoveryLock { get; } = new OwningLock();

        public bool CanDash => Capabilities.CanDash;
        public bool CanJump => Capabilities.CanJump;
        public bool CanLightAttack => Capabilities.CanLightAttack;
        public bool CanStrongAttack => Capabilities.CanStrongAttack;
        public bool CanHeal => Capabilities.CanHeal;

        public Transform Transform => transform;
        public IVelocityMovement VelocityMovement { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public DashCloneEffectController DashEffectController { get; private set; }
        public Animator AnimatorComponent { get; private set; }
        public IHurtbox Hurtbox { get; private set; }
        public IAutoPlayerInput Input { get; private set; }
        public IJumpHandler JumpHandler { get; private set; }
        public MixinPlayerCapabilities Capabilities { get; private set; }
        public MixinAttackExecutorHelper AttackExecutorHelper { get; private set; }
        public MixinInteractor Interactor { get; private set; }
        public GameObject PlayerObject => gameObject;

        public IAttackExecutor FirstLightAttack => lightAttackFirst.GetExecutor();
        public IAttackExecutor SecondLightAttack => lightAttackSecond.GetExecutor();
        public IAttackExecutor AirboneLightAttack => lightAirboneAttack.GetExecutor();
        public IAttackExecutor FirstStrongAttack => firstStrongAttack.GetExecutor();
        public IAttackExecutor SecondStrongAttack => secondStrongAttack.GetExecutor();

        public PlayerConfig PlayerConfig => playerConfig;

        public bool HasDashEffectController { get; private set; } = true;

        public bool AttackedAtThisAirTime { get; set; }
        public IHitReceiver HitReceiver { get; private set; }

        private void Awake()
        {
            Input = GetComponent<IAutoPlayerInput>();
            VelocityMovement = GetComponent<MixinVelocityMovement>().VelocityMovement;

            SpriteRenderer = GetComponent<SpriteRenderer>();
            AnimatorComponent = GetComponent<Animator>();
            DashEffectController = GetComponentInChildren<DashCloneEffectController>();
            HitReceiver = GetComponent<MixinHittable>().HitReceiver;

            Hurtbox = GetComponentInChildren<MonoHurtbox>().Hurtbox;
            JumpHandler = GetComponent<MixinJumpHandler>().JumpHandler;
            Capabilities = GetComponent<MixinPlayerCapabilities>();
            AttackExecutorHelper = GetComponent<MixinAttackExecutorHelper>();
            Interactor = GetComponent<MixinInteractor>();

            if (lightAttackFirst == null)
            {
                Debug.LogWarning("Light Attack First is not assigned", this);
                enabled = false;
            }
            if (lightAttackSecond == null)
            {
                Debug.LogWarning("Light Attack Second is not assigned", this);
                enabled = false;
            }
            if (lightAirboneAttack == null)
            {
                Debug.LogWarning("Light Airbone Attack is not assigned", this);
                enabled = false;
            }
            if (DashEffectController == null)
            {
                Debug.LogWarning("Dash Clone Effect Controller not found", this);
                HasDashEffectController = false;
            }

            var character = Player.Character;
            character.HitReceived += (_, info) => CurrentState.OnHitReceived(info);
            character.Staggered += (_, info) => CurrentState.OnStaggered(info);
            character.Dead += (_, info) => CurrentState.OnDeath(info);
            character.RecoveredFromStagger += _ => CurrentState.OnStaggerEnded();

            Input.Interacted += () => CurrentState.OnInteracted();

            PlayerManager.Instance.PlayerLoaded?.Invoke();
        }

        private void Start()
        {
            // Для корректного определения того, что игрок на земле при загрузке
            VelocityMovement.BaseMovement.Move(Vector3.down * 0.2f);

            var startState = VelocityMovement.BaseMovement.IsGrounded
                ? new PlayerGroundedState() : (PlayerBaseState)new PlayerFallState();
            startState.Init(this);

            CurrentState = startState;
            CurrentState.EnterState();
            CurrentState.UpdateState();

            // Разогревочный
            VelocityMovement.BaseMovement.Move(new Vector3(0.01f, -0.01f, 0));
        }

        private void Update()
        {
            CurrentState.UpdateState();

            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                print(CurrentState);
            }
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                Player.Character.Kill();
            }
        }
    }
}
