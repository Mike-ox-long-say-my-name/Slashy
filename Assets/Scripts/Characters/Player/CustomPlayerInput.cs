using Core.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class CustomPlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerInput unityInput;

        [SerializeField, Min(0)] private float jumpInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float dashInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float attackInputWaitTime = 0.4f;
        [SerializeField, Min(0)] private float healInputWaitTime = 0.4f;

        private readonly TimedTriggerFactory _triggerFactory = new TimedTriggerFactory();

        public TimedTrigger IsJumpPressed { get; private set; }
        public TimedTrigger IsDashPressed { get; private set; }
        public TimedTrigger IsLightAttackPressed { get; private set; }
        public TimedTrigger IsHealPressed { get; private set; }

        public Vector2 MoveInput { get; private set; }

        private void ConnectToUnityInput()
        {
            var actions = new PlayerInputActions();
            actions.Player.Enable();
            actions.Player.Jump.performed += OnJump;
            actions.Player.Dash.performed += OnDash;
            actions.Player.Heal.performed += OnHeal;
            actions.Player.Move.performed += OnMove;
            actions.Player.Move.started += OnMove;
            actions.Player.Move.canceled += OnMove;
            actions.Player.Fire.performed += OnAttack;
        }

        private void Awake()
        {
            IsJumpPressed = _triggerFactory.Create();
            IsDashPressed = _triggerFactory.Create();
            IsLightAttackPressed = _triggerFactory.Create();
            IsHealPressed = _triggerFactory.Create();

            if (unityInput == null)
            {
                Debug.LogWarning("Unity Input is not assigned", this);
                enabled = false;
            }
            else
            {
                ConnectToUnityInput();
            }
        }

        private void Update()
        {
            _triggerFactory.StepAll(Time.deltaTime);
        }

        public void ResetBufferedInput()
        {
            _triggerFactory.ResetAll();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (context.action.IsPressed())
            {
                ResetBufferedInput();
                IsJumpPressed.SetFor(jumpInputWaitTime);
            }
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            if (context.action.IsPressed())
            {
                ResetBufferedInput();
                IsDashPressed.SetFor(dashInputWaitTime);
            }
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (context.action.IsPressed())
            {
                ResetBufferedInput();
                IsLightAttackPressed.SetFor(attackInputWaitTime);
            }
        }

        private void OnHeal(InputAction.CallbackContext context)
        {
            if (context.action.IsPressed())
            {
                ResetBufferedInput();
                IsHealPressed.SetFor(healInputWaitTime);
            }
        }
    }
}