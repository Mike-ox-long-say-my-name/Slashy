namespace Characters.Player.States
{
    public class PlayerFallState : PlayerAirboneState
    {
        public PlayerFallState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void UpdateState()
        {
            HandleGravity();
            HandleAirboneControl();

            CheckStateSwitch();
        }

        private void CheckStateSwitch()
        {
            if (Context.Movement.IsGrounded)
            {
                Context.Movement.ResetXZVelocity();
                SwitchState(Factory.Grounded());
            }
            else if (Context.CanAttack && Context.CanStartAttack && Context.Input.IsLightAttackPressed.CheckAndReset())
            {
                Context.Input.ResetBufferedInput();
                // TODO: SwitchState(Factory.AirLightAttack());
            }
        }
    }
}