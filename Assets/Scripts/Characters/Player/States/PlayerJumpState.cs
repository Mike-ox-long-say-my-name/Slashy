namespace Characters.Player.States
{
    public class PlayerJumpState : PlayerAirboneState
    {
        public override void EnterState()
        {
            base.EnterState();
            
            Context.ResourceSpender.SpendFor(PlayerResourceAction.Jump);
            Context.JumpHandler.Jump();
            Context.JumpAudioSource.Play();
        }

        public override void UpdateState()
        {
            ApplyMoveInput();
            CheckStateSwitch();
        }

        private void CheckStateSwitch()
        {
            if (Context.BaseMovement.IsGrounded)
            {
                SwitchState<PlayerGroundedState>();
            }
            else if (Context.VelocityMovement.Velocity.y < 0)
            {
                SwitchState<PlayerFallState>();
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