namespace Characters.Player.States
{
    public class PlayerJumpState : PlayerAirboneState
    {
        public override void EnterState()
        {
            base.EnterState();
            
            Context.Player.Stamina.Spend(Context.PlayerConfig.JumpStaminaCost);
            Context.JumpHandler.Jump();
        }

        public override void UpdateState()
        {
            HandleControl();

            CheckStateSwitch();
        }

        public virtual void CheckStateSwitch()
        {
            if (Context.VelocityMovement.BaseMovement.IsGrounded)
            {
                SwitchState<PlayerGroundedState>();
            }
            else if (Context.VelocityMovement.Velocity.y < 0)
            {
                SwitchState<PlayerFallState>();
            }
            else if (Context.CanLightAttack
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