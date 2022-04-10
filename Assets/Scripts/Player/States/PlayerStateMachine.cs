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
        [field: SerializeField] public float Gravity { get; private set; } = -9.81f;
        [field: SerializeField] public float GroundedGravity { get; private set; } = -0.2f;
        [field: SerializeField, Min(0)] public float HorizontalMoveSpeed { get; private set; } = 5;
        [field: SerializeField, Min(0)] public float VerticalMoveSpeed { get; private set; } = 2;
        [field: SerializeField, Range(0, 1)] public float AirboneControlFactor { get; private set; } = 0.5f;
        [field: SerializeField, Min(0)] public float JumpStartVelocity { get; private set; } = 5;

        [SerializeField, Min(0)] private float jumpInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float dashInputWaitTime = 0.2f;

        [SerializeField] private Camera followingCamera;
        [SerializeField, Range(0, 1)] private float followSmoothTime = 0.3f;

        [field: Space]
        [field: Header("Components")]
        [field: SerializeField] public CharacterController CharacterController { get; private set; } = null;
        [field: SerializeField] public Hurtbox Hurtbox { get; private set; } = null;

        [field: Space]
        [field: Header("Dash")]
        [field: SerializeField, Min(0)] public float DashRecovery { get; private set; } = 0.3f;
        [field: SerializeField, Min(0)] public float DashDistance { get; private set; } = 3f;
        [field: SerializeField, Min(0)] public float DashTime { get; private set; } = 0.4f;

        [field: Space] [field: SerializeField] public PlayerActionConfig ActionConfig { get; private set; } = null;

        public bool IsDashPressed { get; private set; }
        public bool IsJumpPressed { get; private set; }
        public Vector2 MoveInput { get; private set; }

        private Vector3 _appliedVelocity;

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

        private float _cameraFollowVelocity;

        private void Awake()
        {
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
        }

        private void HandleRotation()
        {
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
            _jumpInputResetTrigger.Step(Time.deltaTime);
            _dashInputResetTrigger.Step(Time.deltaTime);

            if (_jumpInputResetTrigger.CheckAndReset())
            {
                IsJumpPressed = false;
            }
            if (_dashInputResetTrigger.CheckAndReset())
            {
                IsDashPressed = false;
            }
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
            IsJumpPressed = true;
            _jumpInputResetTrigger.SetIn(jumpInputWaitTime);
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!context.action.IsPressed())
            {
                return;
            }
            IsDashPressed = true;
            _dashInputResetTrigger.SetIn(dashInputWaitTime);
        }

        private readonly TimedTrigger _jumpInputResetTrigger = new TimedTrigger();
        private readonly TimedTrigger _dashInputResetTrigger = new TimedTrigger();

        private void MoveCamera()
        {
            var cameraPosition = followingCamera.transform.position;
            var newX = Mathf.SmoothDamp(cameraPosition.x, transform.position.x,
                ref _cameraFollowVelocity, followSmoothTime);
            followingCamera.transform.position = new Vector3(newX, cameraPosition.y, cameraPosition.z);
        }

        private float _horizontalAirboneVelocity;
        private float _verticalAirboneVelocity;

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
