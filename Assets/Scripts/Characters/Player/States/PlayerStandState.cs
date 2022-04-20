namespace Characters.Player.States
{
    public class PlayerGroundedState : PlayerBaseGroundedState
    {
        public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void UpdateState()
        {
            HandleGravity();

            var input = Context.Input.MoveInput;
            Context.AnimatorComponent.SetBool("is-walking", input.sqrMagnitude > 0);

            Context.Movement.Move(input);
            
            CheckStateSwitch();
        }

        protected virtual void CheckStateSwitch()
        {
            if (!Context.Movement.IsGrounded)
            {
                SwitchState(Factory.Fall());
            }
            else if (Context.CanDash && Context.Input.IsDashPressed.CheckAndReset())
            {
                if (Context.PlayerCharacter.HasStamina)
                {
                    SwitchState(Factory.Dash());
                }
            }
            else if (Context.CanJump && Context.Input.IsJumpPressed.CheckAndReset())
            {
                if (Context.PlayerCharacter.HasStamina)
                {
                    SwitchState(Factory.Jump());
                }
            }
            else if (Context.CanHeal && Context.Input.IsHealPressed.CheckAndReset())
            {
                SwitchState(Factory.Heal());
            }
            else if (Context.CanStartAttack && Context.Input.IsLightAttackPressed.CheckAndReset())
            {
                SwitchState(Factory.GroundLightAttack());
            }
        }
    }
}