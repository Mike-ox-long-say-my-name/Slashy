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
            else if (Context.CanLightAttack
                     && Context.CanStartLightAttack
                     && !Context.AttackedAtThisAirTime
                     && Context.AttackExecutorHelper.IsAllIdle()
                     && Context.Player.HasStamina()
                     && Context.Input.IsLightAttackPressed)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerAirLightAttackState>();
            }
        }
    }
}