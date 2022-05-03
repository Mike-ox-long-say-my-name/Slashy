namespace Characters.Player.States
{
    public class PlayerFallState : PlayerAirboneState
    {
        public override void UpdateState()
        {
            HandleControl();

            CheckStateSwitch();
        }

        private void CheckStateSwitch()
        {
            if (Context.VelocityMovement.BaseMovement.IsGrounded)
            {
                SwitchState<PlayerGroundedState>();
            }
            else if (Context.CanAttack && Context.CanStartAttack && Context.Input.IsLightAttackPressed)
            {
                Context.Input.ResetBufferedInput();
                // TODO: SwitchState(Factory.AirLightAttack());
            }
        }
    }
}