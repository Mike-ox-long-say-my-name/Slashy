using Core.Attacking;

namespace Characters.Enemies.Boss
{
    public class BossJumpAttackEnd : BossBaseState
    {
        public override void EnterState()
        {
            Context.Animator.PlayJumpEndAnimation();
            Context.JumpAttackExecutor.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasInterrupted)
            {
                return;
            }

            SwitchState<BossWaitAfterAttack>();
        }
    }
}