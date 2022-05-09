namespace Characters.Player.States
{
    public class PlayerGroundedState : PlayerBaseGroundedState
    {
        public override void EnterState()
        {
            Context.AttackedAtThisAirTime = false;
        }

        public override void UpdateState()
        {
            HandleControl();

            Context.AnimatorComponent.SetBool("is-walking", Context.Input.MoveInput.sqrMagnitude > 0);

            CheckStateSwitch();
        }

        protected virtual void CheckStateSwitch()
        {
            if (!Context.VelocityMovement.BaseMovement.IsGrounded)
            {
                SwitchState<PlayerFallState>();
            }
            else if (Context.CanDash && Context.Input.IsDashPressed)
            {
                Context.Input.ResetBufferedInput();
                if (Context.Player.HasStamina())
                {
                    SwitchState<PlayerDashState>();
                }
            }
            else if (Context.CanJump && Context.Input.IsJumpPressed)
            {
                Context.Input.ResetBufferedInput();
                if (Context.Player.HasStamina())
                {
                    SwitchState<PlayerJumpState>();
                }
            }
            else if (Context.CanHeal && Context.Input.IsHealPressed)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerHealState>();
            }
            else if (Context.CanStartAttack && Context.Input.IsLightAttackPressed)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerGroundLightAttackState>();
            }
            else if (Context.CanStartAttack && Context.Input.IsStrongAttackPressed)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerGroundStrongAttackState>();
            }
        }
    }
}