using System.Collections;
using Attacking;
using Configs;
using Core.Characters;
using Core.Utilities;
using Effects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.States
{
    public class PlayerStateMachine : BasePlayerData
    {
        public PlayerStateFactory StateFactory { get; private set; }
        public PlayerBaseState CurrentState { get; set; }

        [Space]
        [Header("Camera")]
        [SerializeField] private Camera followingCamera;
        [SerializeField, Range(0, 1)] private float followSmoothTime = 0.3f;

        [Space]
        [Header("Player Components")]
        [SerializeField] private CustomPlayerInput customPlayerInput;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerCharacter playerCharacter;
        [SerializeField] private AttackExecutor lightAttackFirst;
        [SerializeField] private AttackExecutor lightAttackSecond;

        [Space]
        [Header("Base Components")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private DashCloneEffectController dashEffectController;
        [SerializeField] private Animator animator;
        [SerializeField] private Hurtbox hurtbox;

        [Space]
        [SerializeField] private PlayerConfig playerConfig;

        public override bool IsInvincible { get; set; }
        public override bool IsJumping => CurrentState.GetType() == typeof(PlayerJumpState);
        public override bool IsDashing => CurrentState.GetType() == typeof(PlayerDashState);
        public override bool IsFalling => CurrentState.GetType() == typeof(PlayerFallState);
        public override bool IsGroundState => CurrentState.GetType() == typeof(PlayerGroundedState);
        public override bool IsAttackState => CurrentState.GetType() == typeof(PlayerGroundLightAttackState);
        public override bool IsStaggered => CurrentState.GetType() == typeof(PlayerAirboneStaggerState)
                                            || CurrentState.GetType() == typeof(PlayerGroundStaggerState);

        public readonly OwningLock CanDash = new OwningLock();
        public readonly OwningLock CanJump = new OwningLock();
        public readonly OwningLock CanAttack = new OwningLock();
        public readonly OwningLock CanHeal = new OwningLock();
        
        public override PlayerMovement Movement => playerMovement;
        public AttackExecutor LightAttackFirst => lightAttackFirst;
        public AttackExecutor LightAttackSecond => lightAttackSecond;
        public SpriteRenderer SpriteRenderer => spriteRenderer;
        public DashCloneEffectController DashEffectController => dashEffectController;
        public override PlayerCharacter PlayerCharacter => playerCharacter;
        public Animator AnimatorComponent => animator;
        public Hurtbox HurtboxComponent => hurtbox;
        public PlayerConfig PlayerConfig => playerConfig;
        public CustomPlayerInput Input => customPlayerInput;

        public bool HasDashEffectController { get; private set; } = true;
        public bool HasHurtbox { get; private set; } = true;
        public bool HasSpriteRenderer { get; private set; } = true;

        private float _cameraFollowVelocity;

        public bool CanStartAttack => CanAttack && !IsAttackState && playerCharacter.HasStamina;


        private IEnumerator Start()
        {
            // Для корректного определения того, что игрок на земле при загрузке
            playerCharacter.OnStaggered.AddListener(() => CurrentState.InterruptState(CharacterInterruption.Staggered));

            playerMovement.MoveRaw(Vector3.down);
            CurrentState = playerMovement.IsGrounded ? StateFactory.Grounded() : StateFactory.Fall();
            CurrentState.EnterState();
            CurrentState.UpdateState();

            yield return new WaitForSeconds(0.3f);
        }

        private void Awake()
        {
            if (customPlayerInput == null)
            {
                Debug.LogWarning("Player Input is not assigned", this);
                enabled = false;
            }
            if (playerMovement == null)
            {
                Debug.LogWarning("Player Movement is not assigned", this);
                enabled = false;
            }
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
            if (playerCharacter == null)
            {
                Debug.LogWarning("Player Character is not assigned", this);
                enabled = false;
            }
            if (hurtbox == null)
            {
                Debug.LogWarning("Hurtbox is not assigned", this);
                HasHurtbox = false;
            }
            if (dashEffectController == null)
            {
                Debug.LogWarning("Dash Clone Effect Controller is not assigned", this);
                HasDashEffectController = false;
            }
            if (spriteRenderer == null)
            {
                Debug.LogWarning("Sprite Renderer is not assigned", this);
                HasSpriteRenderer = false;
            }

            StateFactory = new PlayerStateFactory(this);

            if (followingCamera == null)
            {
                followingCamera = Camera.main;
            }
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
