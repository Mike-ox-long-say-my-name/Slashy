using Characters.Enemies.States;
using Core.Attacking;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowBaseState : EnemyBaseState<WeakHollowStateMachine>
    {
        public override void OnHitReceived(HitInfo info)
        {
            Context.LastHitInfo = info;
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.InterruptActiveAttack();
            Context.LastHitInfo = info;
            SwitchState<WeakHollowStagger>();
        }

        public override void OnDeath(HitInfo info)
        {
            Context.InterruptActiveAttack();
            Context.LastHitInfo = info;
            SwitchState<WeakHollowDeath>();
        }
    }
}