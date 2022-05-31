using Core.Attacking;

namespace Characters.Enemies.Boss
{
    public class BossThrustStationary : BossBaseState
    {
        public override void EnterState()
        {
            Context.Animator.PlayThrustNoDashAnimation();
            Context.ThrustAttackExecutor.StartAttack(OnAttackEnded);
            Context.ThrustAttackConfigurator.SetMaxDashDistance(0);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasCompleted)
            {
                SwitchState<BossWaitAfterAttack>();
            }
        }
    }
}