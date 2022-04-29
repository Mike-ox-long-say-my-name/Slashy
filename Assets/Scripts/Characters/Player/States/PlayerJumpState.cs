namespace Characters.Player.States
{
    public class PlayerJumpState : PlayerAirboneState
    {
        public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            Context.AnimatorComponent.SetTrigger("jump");

            Context.Character.SpendStamina(Context.PlayerConfig.JumpStaminaCost);
            Context.VelocityMovement.Jump();
        }

        public override void UpdateState()
        {
            HandleControl();

            CheckStateSwitch();
        }

        public virtual void CheckStateSwitch()
        {
            if (Context.VelocityMovement.Movement.IsGrounded)
            {
                SwitchState(Factory.Grounded());
            }
            else if (Context.VelocityMovement.Velocity.y < 0)
            {
                SwitchState(Factory.Fall());
            }
            else if (Context.CanStartAttack && Context.Input.IsLightAttackPressed)
            {
                Context.Input.ResetBufferedInput();
                // TODO: SwitchState(Factory.AirLightAttack());
            }
        }
    }
}