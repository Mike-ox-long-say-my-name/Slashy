using Core.Player.Interfaces;
using Core.Utilities;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class CustomPlayerInput : MonoBehaviour, IAutoPlayerInput
    {
        [SerializeField, Min(0)] private float jumpInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float dashInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float attackInputWaitTime = 0.4f;
        [SerializeField, Min(0)] private float healInputWaitTime = 0.4f;

        private readonly TimedTriggerFactory _triggerFactory = new TimedTriggerFactory();

        public TimedTrigger IsJumpPressedTrigger { get; private set; }
        public TimedTrigger IsDashPressedTrigger { get; private set; }
        public TimedTrigger IsLightAttackPressedTrigger { get; private set; }
        public TimedTrigger IsHealPressedTrigger { get; private set; }

        public bool IsJumpPressed => IsJumpPressedTrigger.IsSet;
        public bool IsDashPressed => IsDashPressedTrigger.IsSet;
        public bool IsLightAttackPressed => IsLightAttackPressedTrigger.IsSet;
        public bool IsHealPressed => IsHealPressedTrigger.IsSet;

        public Vector2 MoveInput { get; private set; }

        private void ConnectToUnityInput()
        {
            var actions = PlayerActionsProxy.Instance.Actions;
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
            IsJumpPressedTrigger = _triggerFactory.Create();
            IsDashPressedTrigger = _triggerFactory.Create();
            IsLightAttackPressedTrigger = _triggerFactory.Create();
            IsHealPressedTrigger = _triggerFactory.Create();

            ConnectToUnityInput();
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
            ResetBufferedInput();
            IsJumpPressedTrigger.SetFor(jumpInputWaitTime);
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            ResetBufferedInput();
            IsDashPressedTrigger.SetFor(dashInputWaitTime);
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            ResetBufferedInput();
            IsLightAttackPressedTrigger.SetFor(attackInputWaitTime);
        }

        private void OnHeal(InputAction.CallbackContext context)
        {
            ResetBufferedInput();
            IsHealPressedTrigger.SetFor(healInputWaitTime);
        }
    }
}