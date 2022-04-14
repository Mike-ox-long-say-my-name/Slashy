namespace Player.States
{
    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
        }

        public override void UpdateState()
        {
            Context.ApplyGravity();
            Context.ApplyAirboneMovement();

            CheckStateSwitch();
        }

        public override void ExitState()
        {
        }

        private void CheckStateSwitch()
        {
            if (Context.CharacterController.isGrounded)
            {
                Context.AppliedVelocityX = 0;
                Context.AppliedVelocityZ = 0;
                SwitchState(Factory.Grounded());
            }
            else if (Context.IsLightAttackPressed.CheckAndReset())
            {
                Context.ResetBufferedInput();
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (Context.LightAttackExecutor.IsAttacking)
            {
                return;
            }
            
            // TODO: добавить атаку в прыжке
            // SetSubState(Factory.Attack());
            // SubState.EnterState();
        }
    }
}