namespace Characters.Player.States
{
    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
            IsRootState = true;
        }

        public override void UpdateState()
        {
            Context.Movement.ApplyGravity();

            if (!Context.IsStaggered)
            {
                Context.Movement.ApplyAirboneVelocity(Context.MoveInput);
            }

            CheckStateSwitch();
        }

        private void CheckStateSwitch()
        {
            if (Context.Movement.IsGrounded)
            {
                Context.Movement.ResetXZVelocity();
                SwitchState(Factory.Grounded());
            }
            else if (!Context.IsStaggered && Context.IsLightAttackPressed.CheckAndReset())
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