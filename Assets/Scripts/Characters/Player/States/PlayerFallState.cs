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
            else if (Context.ShouldLightAttack
                     && !Context.AttackedThisAirTime)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerAirLightAttackState>();
            }
        }
    }
}