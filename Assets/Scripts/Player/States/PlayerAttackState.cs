namespace Player.States
{
    public class PlayerAttackState : PlayerBaseState
    {
        public PlayerAttackState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Context.AppliedVelocityX = 0;
            Context.AppliedVelocityZ = 0;
            Context.CanDash = false;
            Context.CanJump = false;

            void AttackEnded(bool _)
            {
                Context.CanDash = true;
                Context.CanJump = true;
                SwitchState(Factory.Idle());
            }

            Context.Animator.SetTrigger("attack-short");
            Context.LightAttackExecutor.StartExecution(Context.PlayerCharacter, AttackEnded);
        }

        public override void UpdateState()
        {
        }

        public override void ExitState()
        {
        }
    }
}