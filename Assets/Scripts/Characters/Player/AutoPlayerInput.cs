using System;
using Core;
using Core.Player.Interfaces;
using Core.Utilities;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class AutoPlayerInput : MonoBehaviour, IAutoPlayerInput
    {
        [SerializeField, Min(0)] private float jumpInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float dashInputWaitTime = 0.2f;
        [SerializeField, Min(0)] private float attackInputWaitTime = 0.4f;
        [SerializeField, Min(0)] private float healInputWaitTime = 0.4f;

        private readonly TimedTriggerFactory _triggerFactory = new TimedTriggerFactory();

        private TimedTrigger _isJumpPressedTrigger;
        private TimedTrigger _isDashPressedTrigger;
        private TimedTrigger _isLightAttackPressedTrigger;
        private TimedTrigger _isHealPressedTrigger;
        private TimedTrigger _isStrongAttackPressedTrigger;
        private PlayerBindings _playerBindings;

        public bool IsJumpPressed => _isJumpPressedTrigger.IsSet;
        public bool IsDashPressed => _isDashPressedTrigger.IsSet;
        public bool IsLightAttackPressed => _isLightAttackPressedTrigger.IsSet;
        public bool IsStrongAttackPressed => _isStrongAttackPressedTrigger.IsSet;
        public bool IsHealPressed => _isHealPressedTrigger.IsSet;

        public event Action Interacted;

        public Vector2 MoveInput { get; private set; }

        private void SubscribeToInput()
        {
            var actions = _playerBindings.Actions;
            actions.Player.Enable();
            actions.Player.Jump.performed += OnJump;
            actions.Player.Dash.performed += OnDash;
            actions.Player.Heal.performed += OnHeal;
            actions.Player.Move.performed += OnMove;
            actions.Player.Move.started += OnMove;
            actions.Player.Move.canceled += OnMove;
            actions.Player.Fire.performed += OnAttack;
            actions.Player.Fire2.performed += OnStrongAttack;
            actions.Player.Interact.performed += OnInteracted;
        }

        private void UnsubscribeFromInput()
        {
            var actions = _playerBindings.Actions;
            actions.Player.Jump.performed -= OnJump;
            actions.Player.Dash.performed -= OnDash;
            actions.Player.Heal.performed -= OnHeal;
            actions.Player.Move.performed -= OnMove;
            actions.Player.Move.started -= OnMove;
            actions.Player.Move.canceled -= OnMove;
            actions.Player.Fire.performed -= OnAttack;
            actions.Player.Fire2.performed -= OnStrongAttack;
            actions.Player.Interact.performed -= OnInteracted;
        }

        private void Awake()
        {
            _isJumpPressedTrigger = _triggerFactory.Create();
            _isDashPressedTrigger = _triggerFactory.Create();
            _isLightAttackPressedTrigger = _triggerFactory.Create();
            _isHealPressedTrigger = _triggerFactory.Create();
            _isStrongAttackPressedTrigger = _triggerFactory.Create();

            _playerBindings = Container.Get<PlayerBindings>();
        }

        private void OnEnable()
        {
            SubscribeToInput();
        }

        private void OnDisable()
        {
            UnsubscribeFromInput();
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
            _isJumpPressedTrigger.SetFor(jumpInputWaitTime);
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            ResetBufferedInput();
            _isDashPressedTrigger.SetFor(dashInputWaitTime);
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            ResetBufferedInput();
            _isLightAttackPressedTrigger.SetFor(attackInputWaitTime);
        }

        private void OnStrongAttack(InputAction.CallbackContext context)
        {
            ResetBufferedInput();
            _isStrongAttackPressedTrigger.SetFor(attackInputWaitTime);
        }

        private void OnHeal(InputAction.CallbackContext context)
        {
            ResetBufferedInput();
            _isHealPressedTrigger.SetFor(healInputWaitTime);
        }

        private void OnInteracted(InputAction.CallbackContext context)
        {
            Interacted?.Invoke();
        }
    }
}