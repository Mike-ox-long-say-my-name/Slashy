namespace Characters.Player.States
{
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            if (!Context.IsStaggered)
            {
                SwitchSubState(Context.MoveInput.sqrMagnitude > 0 ? Factory.Walk() : Factory.Idle());
            }
        }

        public override void UpdateState()
        {
            Context.Movement.ApplyGravity();

            CheckStateSwitch();
        }

        private void CheckStateSwitch()
        {
            if (Context.IsStaggered || Context.IsAttackState)
            {
                return;
            }

            if (!Context.Movement.IsGrounded)
            {
                SwitchState(Factory.Fall());
            }
            else if (Context.CanDash && Context.IsDashPressed.CheckAndReset())
            {
                Context.ResetBufferedInput();
                if (Context.Player.HasStamina)
                {
                    SwitchState(Factory.Dash());
                }
            }
            else if (Context.CanJump && Context.IsJumpPressed.CheckAndReset())
            {
                Context.ResetBufferedInput();
                if (Context.Player.HasStamina)
                {
                    SwitchState(Factory.Jump());
                }
            }
            else if (Context.IsHealPressed.CheckAndReset())
            {
                Context.ResetBufferedInput();
                SwitchState(Factory.Heal());
            }
            else if (Context.CanAttack && Context.IsLightAttackPressed.CheckAndReset())
            {
                Context.ResetBufferedInput();
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (!Context.CanStartAttack)
            {
                return;
            }

            SetSubState(Factory.Attack());
            SubState.EnterState();
        }
    }
}