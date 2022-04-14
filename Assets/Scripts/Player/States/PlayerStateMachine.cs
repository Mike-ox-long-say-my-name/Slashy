using Attacking;
using Configs;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player.States
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public PlayerStateFactory StateFactory { get; private set; }
        public PlayerBaseState CurrentState { get; set; }

        [field: Space]
        [field: Header("Movement")]
        [field: SerializeField] public float Gravity { get; set; } = -9.81f;
        [field: SerializeField] public float GroundedGravity { get; set; } = -0.2f;
        [field: SerializeField, Min(0)] public float HorizontalMoveSpeed { get; set; } = 5;
        [field: SerializeField, Min(0)] public float VerticalMoveSpeed { get; set; } = 2;
        [field: SerializeField, Range(0, 1)] public float AirboneControlFactor { get; set; } = 0.5f;
        [field: SerializeField, Min(0)] public float JumpStartVelocity { get; set; } = 5;

        [Space]
        [SerializeField, Min(0)] private float jumpInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float dashInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float attackInputWaitTime = 0.2f;
        
        [Space]
        [SerializeField] private Camera followingCamera;
        [SerializeField, Range(0, 1)] private float followSmoothTime = 0.3f;

        [field: Space]
        [field: Header("Components")]
        [field: SerializeField] public CharacterController CharacterController { get; set; }
        [field: SerializeField] public AttackExecutor LightAttackExecutor { get; set; }
        [field: SerializeField] public PlayerCharacter PlayerCharacter { get; set; }

        [field: SerializeField] public Animator Animator { get; set; } = null;
        [field: SerializeField] public Hurtbox Hurtbox { get; set; } = null;

        [field: Space]
        [field: Header("Dash")]
        [field: SerializeField, Min(0)] public float DashRecovery { get; set; } = 0.3f;
        [field: SerializeField, Min(0)] public float DashDistance { get; set; } = 3f;
        [field: SerializeField, Min(0)] public float DashTime { get; set; } = 0.4f;

        [field: Space]
        [field: SerializeField] public PlayerActionConfig ActionConfig { get; set; } = null;

        public Vector2 MoveInput { get; private set; }

        public Vector3 AppliedVelocity { get => _appliedVelocity; set => _appliedVelocity = value; }
        public float AppliedVelocityX { get => _appliedVelocity.x; set => _appliedVelocity.x = value; }
        public float AppliedVelocityY { get => _appliedVelocity.y; set => _appliedVelocity.y = value; }
        public float AppliedVelocityZ { get => _appliedVelocity.z; set => _appliedVelocity.z = value; }

        public bool IsInvincible { get; set; }
        public bool IsJumping => CurrentState.GetType() == typeof(PlayerJumpState);
        public bool IsDashing => CurrentState.GetType() == typeof(PlayerDashState);
        public bool IsFalling => CurrentState.GetType() == typeof(PlayerFallState);
        public bool IsGrounded => CurrentState.GetType() == typeof(PlayerGroundedState);
        public bool IsWalking => CurrentState.GetType() == typeof(PlayerWalkState);
        public bool IsIdle => CurrentState.GetType() == typeof(PlayerIdleState);

        public bool CanDash { get; set; } = true;
        public bool CanJump { get; set; } = true;

        private Vector3 _appliedVelocity;
        private float _cameraFollowVelocity;

        private readonly TimedTriggerFactory _triggerFactory = new TimedTriggerFactory();

        public TimedTrigger IsJumpPressed { get; private set; }
        public TimedTrigger IsDashPressed { get; private set; }
        public TimedTrigger IsLightAttackPressed { get; private set; }
        public bool IsAttacking => LightAttackExecutor.IsAttacking;


        private void Awake()
        {
            IsJumpPressed = _triggerFactory.Create();
            IsDashPressed = _triggerFactory.Create();
            IsLightAttackPressed = _triggerFactory.Create();

            StateFactory = new PlayerStateFactory(this);
            CurrentState = CharacterController.isGrounded ? StateFactory.Grounded() : StateFactory.Fall();
            CurrentState.EnterState();

            if (followingCamera == null)
            {
                followingCamera = Camera.main;
            }
        }

        private void Update()
        {
            CurrentState.UpdateStates();

            HandleRotation();
            HandleTriggers();

            CharacterController.Move(Time.deltaTime * AppliedVelocity);

            MoveCamera();

            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                print($"{CurrentState.GetType()}");
            }
        }

        private void HandleRotation()
        {
            if (IsDashing)
            {
                return;
            }
            if (MoveInput.x > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (MoveInput.x < 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
        }

        private void HandleTriggers()
        {
            _triggerFactory.StepAll(Time.deltaTime);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.action.IsPressed())
            {
                return;
            }
            IsJumpPressed.SetFor(jumpInputWaitTime);
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!context.action.IsPressed())
            {
                return;
            }
            IsDashPressed.SetFor(dashInputWaitTime);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            IsLightAttackPressed.SetFor(attackInputWaitTime);
        }

        private void MoveCamera()
        {
            var cameraPosition = followingCamera.transform.position;
            var newX = Mathf.SmoothDamp(cameraPosition.x, transform.position.x,
                ref _cameraFollowVelocity, followSmoothTime);
            followingCamera.transform.position = new Vector3(newX, cameraPosition.y, cameraPosition.z);
        }

        private float _horizontalAirboneVelocity;
        private float _verticalAirboneVelocity;

        public void ResetBufferedInput()
        {
            _triggerFactory.ResetAll();
        }

        public void ApplyGravity()
        {
            AppliedVelocityY += (CharacterController.isGrounded ? GroundedGravity : Gravity) * Time.deltaTime;
        }

        public void ApplyAirboneMovement()
        {
            var targetHorizontalVelocity = HorizontalMoveSpeed * MoveInput.x;
            var targetVerticalVelocity = VerticalMoveSpeed * MoveInput.y;
            AppliedVelocityX = Mathf.SmoothDamp(AppliedVelocityX, targetHorizontalVelocity,
                ref _horizontalAirboneVelocity, AirboneControlFactor);
            AppliedVelocityZ = Mathf.SmoothDamp(AppliedVelocityZ, targetVerticalVelocity,
                ref _verticalAirboneVelocity, AirboneControlFactor);
        }
    }
}
