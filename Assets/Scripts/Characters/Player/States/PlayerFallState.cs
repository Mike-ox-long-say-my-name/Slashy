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

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-airbone", false);
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

            // TODO: �������� ����� � ������
            // SetSubState(Factory.Attack());
            // SubState.EnterState();
        }
    }
}