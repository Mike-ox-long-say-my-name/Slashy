namespace Characters.Player.States
{
    public class PlayerGroundedState : PlayerBaseGroundedState
    {
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
            if (!Context.VelocityMovement.BaseMovement.IsGrounded)
            {
                SwitchState<PlayerFallState>();
            }
            else if (Context.CanDash
                     && Context.DashRecoveryLock.IsUnlocked
                     && Context.Input.IsDashPressed
                     && Context.Player.HasStamina())
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerDashState>();
            }
            else if (Context.CanJump
                     && Context.Input.IsJumpPressed
                     && Context.Player.HasStamina())
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerJumpState>();
            }
            else if (Context.CanHeal
                     && Context.Input.IsHealPressed
                     && Context.Player.HasStamina())
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerHealState>();
            }
            else if (Context.CanLightAttack
                     && Context.AttackExecutorHelper.IsAllIdle()
                     && Context.Input.IsLightAttackPressed
                     && Context.Player.HasStamina())
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerGroundLightAttackState>();
            }
            else if (Context.CanStrongAttack
                     && Context.AttackExecutorHelper.IsAllIdle()
                     && Context.Input.IsStrongAttackPressed
                     && Context.Player.HasStamina())
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerGroundStrongAttackState>();
            }
        }
    }
}