using Core.Attacking;
using Core.Characters.Interfaces;

namespace Characters.Enemies
{
    public class ExplodingHollowStagger : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            //Context.VelocityMovement.Stop();

            Context.Animator.SetBool("is-staggered", true);
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-staggered", false);
        }

        public override void OnStaggerEnded()
        {
            if (Context.WasHitByPlayer)
            {
                SwitchState<ExplodingHollowCharging>();
            }
            else
            {
                SwitchState<ExplodingHollowIdle>();
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