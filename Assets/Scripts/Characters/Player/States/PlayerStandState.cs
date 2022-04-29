namespace Characters.Player.States
{
    public class PlayerGroundedState : PlayerBaseGroundedState
    {
        public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

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
            if (!Context.VelocityMovement.Movement.IsGrounded)
            {
                SwitchState(Factory.Fall());
            }
            else if (Context.CanDash && Context.Input.IsDashPressed)
            {
                Context.Input.ResetBufferedInput();
                if (Context.Character.HasStamina())
                {
                    SwitchState(Factory.Dash());
                }
            }
            else if (Context.CanJump && Context.Input.IsJumpPressed)
            {
                Context.Input.ResetBufferedInput();
                if (Context.Character.HasStamina())
                {
                    SwitchState(Factory.Jump());
                }
            }
            else if (Context.CanHeal && Context.Input.IsHealPressed)
            {
                Context.Input.ResetBufferedInput();
                SwitchState(Factory.Heal());
            }
            else if (Context.CanStartAttack && Context.Input.IsLightAttackPressed)
            {
                Context.Input.ResetBufferedInput();
                SwitchState(Factory.GroundLightAttack());
            }
        }
    }
}