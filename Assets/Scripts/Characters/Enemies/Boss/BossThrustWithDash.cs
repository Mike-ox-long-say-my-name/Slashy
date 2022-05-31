using Core.Attacking;

namespace Characters.Enemies.Boss
{
    public class BossThrustWithDash : BossBaseState
    {
        public override void EnterState()
        {
            Context.PlayerAwareness.PostDangerEvent();
            Context.Animator.PlayThrustWithDashAnimation();
            Context.ThrustAttackExecutor.StartAttack(OnAttackEnded);
            Context.ThrustAttackConfigurator.SetMaxDashDistance(Context.MaxDashDistance);
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