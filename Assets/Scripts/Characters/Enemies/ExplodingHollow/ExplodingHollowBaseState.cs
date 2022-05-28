using Characters.Enemies.States;
using Core.Attacking;
using Core.Characters.Interfaces;

namespace Characters.Enemies.ExplodingHollow
{
    public abstract class ExplodingHollowBaseState : EnemyBaseState<ExplodingHollowStateMachine>
    {
        public override void OnDeath(HitInfo info)
        {
            SwitchState<ExplodingHollowDeath>(true);
        }

        public override void OnHitReceived(HitInfo info)
        {
            if (!(info.Source.Character is { Team: Team.Player }))
            {
                return;
            }

            Context.WasHitByPlayer = true;
            SwitchState<ExplodingHollowCharging>();
        }

        public override void OnStaggered(HitInfo info)
        {
            if (!(info.Source.Character is { Team: Team.Player }))
            {
                SwitchState<ExplodingHollowStagger>(true);
                return;
            }

            Context.WasHitByPlayer = true;
            SwitchState<ExplodingHollowCharging>();
        }
    }
}