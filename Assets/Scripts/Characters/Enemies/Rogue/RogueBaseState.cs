using Characters.Enemies.States;
using Core.Attacking;

namespace Characters.Enemies.Rogue
{
    public class RogueBaseState : EnemyBaseState<RogueStateMachine>
    {
        public override void OnHitReceived(HitInfo info)
        {
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<RogueStaggered>(true);
        }

        public override void OnDeath(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<RogueDeath>(true);
        }
    }
}