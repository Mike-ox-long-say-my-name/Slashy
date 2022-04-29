namespace Characters.Player.States
{
    public class PlayerFallState : PlayerAirboneState
    {
        public PlayerFallState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void UpdateState()
        {
            HandleControl();

            CheckStateSwitch();
        }

        private void CheckStateSwitch()
        {
            if (Context.VelocityMovement.Movement.IsGrounded)
            {
                SwitchState(Factory.Grounded());
            }
            else if (Context.CanAttack && Context.CanStartAttack && Context.Input.IsLightAttackPressed)
            {
                Context.Input.ResetBufferedInput();
                // TODO: SwitchState(Factory.AirLightAttack());
            }
        }
    }
}