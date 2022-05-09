using Characters.Enemies.States;
using Core.Attacking;
using Core.Characters.Interfaces;

namespace Characters.Enemies.ExplodingHollow
{
    public abstract class ExplodingHollowBaseState : EnemyBaseState<ExplodingHollowStateMachine>
    {
        public override void OnDeath(HitInfo info)
        {
            SwitchState<ExplodingHollowDeath>();
        }

        public override void OnHitReceived(HitInfo info)
        {
            if (info.Source.Character.Team != Team.Player)
            {
                return;
            }
            Context.WasHitByPlayer = true;
            SwitchState<ExplodingHollowCharging>();
        }

        public override void OnStaggered(HitInfo info)
        {
            if (info.Source.Character.Team != Team.Player)
            {
                SwitchState<ExplodingHollowStagger>();
                return;
            }
            Context.WasHitByPlayer = true;
            SwitchState<ExplodingHollowCharging>();
        }
    }
}