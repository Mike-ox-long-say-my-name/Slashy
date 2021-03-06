using Core.Attacking;
using Core.Characters.Interfaces;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowStagger : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.Animator.StartStaggerAnimation();
        }

        public override void ExitState()
        {
            Context.Animator.EndStaggerAnimation();
        }

        public override void OnStaggerEnded()
        {
            if (Context.WasHitByPlayer)
            {
                SwitchState<ExplodingHollowCharging>();
            }
            else
            {
                SwitchState<ExplodingHollowPursue>();
            }
        }

        public override void OnHitReceived(HitInfo info)
        {
            var character = info.Source.Character;
            if (character is { Team: Team.Player })
            {
                Context.WasHitByPlayer = true;
            }
        }
    }
}