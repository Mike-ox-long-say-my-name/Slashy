using Characters.Enemies.States;
using Core.Attacking;

namespace Characters.Enemies.Boss
{
    public class BossBaseState : EnemyBaseState<BossStateMachine>
    {
        public override void OnStaggered(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<BossStaggered>(true);
        }

        public override void OnDeath(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<BossDeath>(true);
        }
    }
}