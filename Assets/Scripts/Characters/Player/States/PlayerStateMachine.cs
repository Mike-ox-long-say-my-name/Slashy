using Configs;
using Core.Utilities;
using Effects;
using System.Collections;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Player.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.States
{
    public class PlayerStateMachine : MonoBehaviour, IMonoPlayerInfoProvider
    {
        public PlayerStateFactory StateFactory { get; private set; }
        public PlayerBaseState CurrentState { get; set; }

        [Space]
        [Header("Camera")]
        [SerializeField] private Camera followingCamera;
        [SerializeField, Range(0, 1)] private float followSmoothTime = 0.3f;

        [Space]
        [Header("Attacks")]
        [SerializeField] private MonoAttackHandler lightMonoAttackFirst;
        [SerializeField] private MonoAttackHandler lightMonoAttackSecond;

        [Space]
        [SerializeField] private PlayerConfig playerConfig;

        public bool IsInvincible { get; set; }
        public bool IsJumping => CurrentState.GetType() == typeof(PlayerJumpState);
        public bool IsDashing => CurrentState.GetType() == typeof(PlayerDashState);
        public bool IsFalling => CurrentState.GetType() == typeof(PlayerFallState);
        public bool IsStaggered => CurrentState.GetType() == typeof(PlayerAirboneStaggerState)
                                            || CurrentState.GetType() == typeof(PlayerGroundStaggerState);

        public bool IsAttacking => CurrentState.GetType() == typeof(PlayerGroundLightAttackState);

        private IMonoPlayerCharacter _player;
        public IMonoPlayerCharacter Player => _player ??= GetComponent<IMonoPlayerCharacter>();

        public readonly OwningLock CanDash = new OwningLock();
        public readonly OwningLock CanJump = new OwningLock();
        public readonly OwningLock CanAttack = new OwningLock();
        public readonly OwningLock CanHeal = new OwningLock();

        public IPlayerMovement Movement => Character.Movement;
        public SpriteRenderer SpriteRenderer { get; private set; }
        public DashCloneEffectController DashEffectController { get; private set; }
        public IPlayerCharacter Character { get; private set; }
        public Animator AnimatorComponent { get; private set; }
        public IHurtbox Hurtbox { get; private set; }
        public IAutoPlayerInput Input { get; private set; }

        public IAttackExecutor LightMonoAttackFirst { get; private set; }
        public IAttackExecutor LightMonoAttackSecond { get; private set; }
        public PlayerConfig PlayerConfig => playerConfig;

        public bool HasDashEffectController { get; private set; } = true;
        public bool HasSpriteRenderer { get; private set; } = true;

        private float _cameraFollowVelocity;

        public bool CanStartAttack => CanAttack && !IsAttacking && Character.HasStamina();

        private void Awake()
        {
            Input = GetComponent<IAutoPlayerInput>();

            SpriteRenderer = GetComponent<SpriteRenderer>();
            AnimatorComponent = GetComponent<Animator>();
            DashEffectController = GetComponentInChildren<DashCloneEffectController>();

            if (lightMonoAttackFirst == null)
            {
                Debug.LogWarning("Light Attack First is not assigned", this);
                enabled = false;
            }
            if (lightMonoAttackSecond == null)
            {
                Debug.LogWarning("Light Attack Second is not assigned", this);
                enabled = false;
            }
            if (DashEffectController == null)
            {
                Debug.LogWarning("Dash Clone Effect Controller not found", this);
                HasDashEffectController = false;
            }
            if (SpriteRenderer == null)
            {
                Debug.LogWarning("Sprite Renderer not found", this);
                HasSpriteRenderer = false;
            }

            StateFactory = new PlayerStateFactory(this);

            if (followingCamera == null)
            {
                followingCamera = Camera.main;
            }

            LightMonoAttackFirst = lightMonoAttackFirst.Resolve();
            LightMonoAttackSecond = lightMonoAttackSecond.Resolve();

            Hurtbox = GetComponentInChildren<IMonoHurtbox>()?.Resolve();

            var character = Player;
            character.OnHitReceived.AddListener((_, info) => CurrentState.OnHitReceived(info));
            character.OnStaggered.AddListener((_, info) => CurrentState.OnStaggered(info));
            character.OnDeath.AddListener((_, info) => CurrentState.OnDeath(info));
            Character = character.Resolve();
        }

        private IEnumerator Start()
        {
            // Для корректного определения того, что игрок на земле при загрузке
            Movement.MoveRaw(Vector3.down);
            CurrentState = Movement.IsGrounded ? StateFactory.Grounded() : StateFactory.Fall();
            CurrentState.EnterState();
            CurrentState.UpdateState();

            yield return new WaitForSeconds(0.3f);
        }

        private void Update()
        {
            CurrentState.UpdateState();
            MoveCamera();

            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                print(CurrentState);
            }
        }

        private void MoveCamera()
        {
            var cameraPosition = followingCamera.transform.position;
            var newX = Mathf.SmoothDamp(cameraPosition.x, transform.position.x,
                ref _cameraFollowVelocity, followSmoothTime);
            followingCamera.transform.position = new Vector3(newX, cameraPosition.y, cameraPosition.z);
        }
    }
}
