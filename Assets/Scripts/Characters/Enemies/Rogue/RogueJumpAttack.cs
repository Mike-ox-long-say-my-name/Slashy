using Core.Attacking;

namespace Characters.Enemies.Rogue
{
    public class RogueJumpAttack : RogueBaseState
    {
        private bool _updatePassed;

        public override void EnterState()
        {
            Context.Animator.PlayJumpAttackAnimation();
            _updatePassed = false;
            Context.JumpAttackExecutor.StartAttack(OnAttackEnded);
        }

        public override void UpdateState()
        {
            if (_updatePassed && Context.BaseMovement.IsGrounded)
            {
                if (Context.JumpAttackExecutor.IsAttacking)
                {
                    Context.JumpAttackExecutor.InterruptAttack();
                }

                SwitchState<RogueWaitAfterJumpAttack>();
            }

            _updatePassed = true;
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasCompleted || !Context.BaseMovement.IsGrounded)
            {
                Context.Animator.StartJumpAwayAnimation();
            }
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<RogueAirStagger>();
        }

        public override void ExitState()
        {
            Context.Animator.EndJumpAwayAnimation();
        }
    }
}