namespace Player.States
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            Context.Player.SpendStamina(Context.ActionConfig.JumpStaminaCost);
            Context.AnimatorComponent.SetTrigger("jump");

            Context.Movement.Jump();
        }

        public override void UpdateState()
        {
            Context.Movement.ApplyGravity();
            Context.Movement.ApplyAirboneVelocity(Context.MoveInput);

            CheckStateSwitch();
        }

        public virtual void CheckStateSwitch()
        {
            if (Context.Movement.IsGrounded)
            {
                SwitchState(Factory.Grounded());
            }
            else if (Context.Movement.Velocity.y < 0)
            {
                SwitchState(Factory.Fall());
            }
            else if (Context.IsLightAttackPressed.CheckAndReset())
            {
                Context.ResetBufferedInput();
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (!Context.CanStartAttack)
            {
                return;
            }

            // TODO: добавить атаку в прыжке
            // SetSubState(Factory.Attack());
            // SubState.EnterState();
        }
    }
}