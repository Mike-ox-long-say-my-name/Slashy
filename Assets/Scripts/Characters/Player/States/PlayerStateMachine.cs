using System;
using Configs.Player;
using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Levels;
using Core.Modules;
using Core.Player.Interfaces;
using Core.Utilities;
using Effects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.States
{
    [RequireComponent(typeof(MixinVelocityMovement))]
    [RequireComponent(typeof(MixinStamina))]
    [RequireComponent(typeof(MixinCharacter))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(MixinJumpHandler))]
    [RequireComponent(typeof(MixinHittable))]
    public class PlayerStateMachine : MonoBehaviour, IPlayer
    {
        public PlayerBaseState CurrentState { get; set; }

        [Space] [Header("Attacks")] [SerializeField]
        private MonoAbstractAttackExecutor lightAttackFirst;

        [SerializeField] private MonoAbstractAttackExecutor lightAttackSecond;
        [SerializeField] private MonoAbstractAttackExecutor lightAirboneAttack;
        [SerializeField] private MonoAbstractAttackExecutor firstStrongAttack;
        [SerializeField] private MonoAbstractAttackExecutor secondStrongAttack;

        [Space] [Header("Sounds")] [SerializeField]
        private AudioSource jumpAudioSource;

        [SerializeField] private AudioSource healAudioSource;
        [SerializeField] private AudioSource dashAudioSource;

        [Space] [SerializeField] private PlayerConfig playerConfig;
        private ILevelWarper _levelWarper;

        public AudioSource JumpAudioSource => jumpAudioSource;
        public AudioSource HealAudioSource => healAudioSource;
        public AudioSource DashAudioSource => dashAudioSource;

        public bool IsInvincible { get; set; }

        public OwningLock DashRecoveryLock { get; } = new OwningLock();

        public Transform Transform => transform;
        public ICharacter Character { get; private set; }
        public IResource Stamina { get; private set; }
        public IVelocityMovement VelocityMovement { get; private set; }
        public IBaseMovement BaseMovement => VelocityMovement.BaseMovement;
        public SpriteRenderer SpriteRenderer { get; private set; }
        public DashCloneEffectController DashEffectController { get; private set; }
        public IPlayerAnimator Animator { get; private set; }
        public IHurtbox Hurtbox { get; private set; }
        public IAutoPlayerInput Input { get; private set; }
        public IJumpHandler JumpHandler { get; private set; }
        public AttackExecutorHelper AttackExecutorHelper { get; private set; }
        public Interactor Interactor { get; private set; }
        public GameObject Object => gameObject;
        public bool IsFrozen { get; set; }

        public void Freeze()
        {
            IsFrozen = true;
        }

        public void Unfreeze()
        {
            IsFrozen = false;
        }

        public Bonfire BonfireToTouch { get; set; }
        public event Action TouchedBonfire;
        public event Action<Vector3> StartedWarping;

        public void StartWarp(Vector3 position) => OnStartedWarping(position);

        public void EndWarp(Vector3 target) => OnEndedWarping(target);

        private void OnEndedWarping(Vector3 target) => CurrentState.OnWarpEnded(target);

        public IAttackExecutor FirstLightAttack => lightAttackFirst.GetExecutor();
        public IAttackExecutor SecondLightAttack => lightAttackSecond.GetExecutor();
        public IAttackExecutor AirboneLightAttack => lightAirboneAttack.GetExecutor();
        public IAttackExecutor FirstStrongAttack => firstStrongAttack.GetExecutor();
        public IAttackExecutor SecondStrongAttack => secondStrongAttack.GetExecutor();

        public PlayerConfig PlayerConfig => playerConfig;

        public bool AttackedThisAirTime { get; set; }
        public IHitReceiver HitReceiver { get; private set; }

        public bool ShouldLightAttack => AttackExecutorHelper.IsAllIdle()
                                         && Stamina.HasAny()
                                         && Input.IsLightAttackPressed;

        public bool ShouldStrongAttack => AttackExecutorHelper.IsAllIdle()
                                          && Stamina.HasAny()
                                          && Input.IsStrongAttackPressed;

        public bool ShouldDash => DashRecoveryLock.IsUnlocked
                                  && Input.IsDashPressed
                                  && Stamina.HasAny();

        public bool ShouldJump => Input.IsJumpPressed
                                  && Stamina.HasAny();

        public bool ShouldHeal => Input.IsHealPressed
                                  && Stamina.HasAny();

        public Vector3? WarpPosition { get; set; }
        public Vector3 Position => Transform.position;
        public IPlayerActionResourceSpender ResourceSpender { get; private set; }
        public IPlayerDeathSequencePlayer DeathSequencePlayer { get; private set; }


        private void Awake()
        {
            Character = GetComponent<MixinCharacter>().Character;
            Stamina = GetComponent<MixinStamina>().Resource;
            Input = GetComponent<IAutoPlayerInput>();
            VelocityMovement = GetComponent<MixinVelocityMovement>().VelocityMovement;

            SpriteRenderer = GetComponent<SpriteRenderer>();
            DashEffectController = GetComponentInChildren<DashCloneEffectController>();
            HitReceiver = GetComponent<MixinHittable>().HitReceiver;

            Hurtbox = GetComponentInChildren<MonoHurtbox>().Hurtbox;
            JumpHandler = GetComponent<MixinJumpHandler>().JumpHandler;

            Construct();

            SubscribeToCharacterEvents();
            SubscribeToInputInteraction();
        }

        private void Start()
        {
            var startState = BaseMovement.IsGrounded
                ? new PlayerGroundedState()
                : (PlayerBaseState)new PlayerFallState();
            startState.Init(this);
            CurrentState = startState;

            CurrentState.EnterState();

            if (_levelWarper.IsWarping)
            {
                _levelWarper.ConfirmWarpEnd();
            }
        }

        private void Construct()
        {
            var baseAnimator = GetComponent<Animator>();
            Animator = new PlayerAnimator(baseAnimator);
            DeathSequencePlayer = Container.Get<IPlayerDeathSequencePlayer>();

            ResourceSpender = new PlayerActionResourceSpender(playerConfig, Character.Health, Stamina);
            AttackExecutorHelper = new AttackExecutorHelper(
                GetComponentsInChildren<MonoAbstractAttackExecutor>());
            Interactor = new Interactor(Container.Get<IInteractionService>());

            _levelWarper = Container.Get<ILevelWarper>();
        }

        private void SubscribeToInputInteraction()
        {
            Input.Interacted += () => CurrentState.OnInteracted();
        }

        private void SubscribeToCharacterEvents()
        {
            Character.HitReceived += info => CurrentState.OnHitReceived(info);
            Character.Staggered += info => CurrentState.OnStaggered(info);
            Character.Died += info => CurrentState.OnDeath(info);
            Character.RecoveredFromStagger += () => CurrentState.OnStaggerEnded();
        }

        private void OnStartedWarping(Vector3 position)
        {
            StartedWarping?.Invoke(position);
            CurrentState.OnWarpStarted(position);
        }

        private void Update()
        {
            if (!IsFrozen)
            {
                CurrentState.UpdateState();
            }

            HandleMiscInput();
        }

        private void HandleMiscInput()
        {
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                print(CurrentState);
            }

            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                Character.Kill();
            }
        }

        public void OnTouchedBonfire()
        {
            TouchedBonfire?.Invoke();
        }
    }
}