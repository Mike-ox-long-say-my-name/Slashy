using Attacking;
using Configs;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player.States
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public PlayerStateFactory StateFactory { get; private set; }
        public PlayerBaseState CurrentState { get; set; }

        [Space]
        [SerializeField, Min(0)] private float jumpInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float dashInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float attackInputWaitTime = 0.2f;

        [Space]
        [SerializeField] private Camera followingCamera;
        [SerializeField, Range(0, 1)] private float followSmoothTime = 0.3f;

        [Space]
        [Header("Components")]
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private AttackExecutor lightAttackFirst;
        [SerializeField] private AttackExecutor lightAttackSecond;
        [SerializeField] private PlayerCharacter playerCharacter;
        [SerializeField] private Animator animator;
        [SerializeField] private Hurtbox hurtbox;

        [Space]
        [Header("Dash")]
        [SerializeField, Min(0)] private float dashRecovery = 0.3f;
        [SerializeField, Min(0)] private float dashDistance = 3f;
        [SerializeField, Min(0)] private float dashTime = 0.4f;

        [Space]
        [SerializeField] private PlayerActionConfig actionConfig;

        public Vector2 MoveInput { get; private set; }

        public bool IsInvincible { get; set; }
        public bool IsJumping => CurrentState.GetType() == typeof(PlayerJumpState);
        public bool IsDashing => CurrentState.GetType() == typeof(PlayerDashState);
        public bool IsFalling => CurrentState.GetType() == typeof(PlayerFallState);
        public bool IsGrounded => CurrentState.GetType() == typeof(PlayerGroundedState);
        public bool IsWalking => CurrentState.GetType() == typeof(PlayerWalkState);
        public bool IsIdle => CurrentState.GetType() == typeof(PlayerIdleState);

        public readonly PersistentLock CanDash = new PersistentLock();
        public readonly PersistentLock CanJump = new PersistentLock();
        public readonly PersistentLock CanAttack = new PersistentLock();

        private float _cameraFollowVelocity;

        public PlayerMovement Movement => movement;

        private readonly TimedTriggerFactory _triggerFactory = new TimedTriggerFactory();

        public TimedTrigger IsJumpPressed { get; private set; }
        public TimedTrigger IsDashPressed { get; private set; }
        public TimedTrigger IsLightAttackPressed { get; private set; }

        public bool IsAttacking => lightAttackFirst.IsAttacking || lightAttackSecond.IsAttacking;
        public bool CanStartAttack => !IsAttacking && playerCharacter.HasStamina;

        public AttackExecutor LightAttackFirst => lightAttackFirst;

        public AttackExecutor LightAttackSecond => lightAttackSecond;

        public PlayerCharacter Player => playerCharacter;

        public Animator AnimatorComponent => animator;

        public Hurtbox HurtboxComponent => hurtbox;

        public PlayerActionConfig ActionConfig => actionConfig;

        public float DashRecovery => dashRecovery;

        public float DashDistance => dashDistance;

        public float DashTime => dashTime;


        private void Awake()
        {
            if (movement == null)
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
                enabled = false;
            }

            if (!enabled)
            {
                return;
            }

            IsJumpPressed = _triggerFactory.Create();
            IsDashPressed = _triggerFactory.Create();
            IsLightAttackPressed = _triggerFactory.Create();

            StateFactory = new PlayerStateFactory(this);

            // ��� ����������� ����������� ����, ��� ����� �� ����� ��� ��������
            movement.MoveRaw(Vector3.down);
            CurrentState = movement.IsGrounded ? StateFactory.Grounded() : StateFactory.Fall();
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
            if (!context.action.IsPressed())
            {
                return;
            }
            IsLightAttackPressed.SetFor(attackInputWaitTime);
        }

        private void MoveCamera()
        {
            var cameraPosition = followingCamera.transform.position;
            var newX = Mathf.SmoothDamp(cameraPosition.x, transform.position.x,
                ref _cameraFollowVelocity, followSmoothTime);
            followingCamera.transform.position = new Vector3(newX, cameraPosition.y, cameraPosition.z);
        }

        public void ResetBufferedInput()
        {
            _triggerFactory.ResetAll();
        }
    }
}
