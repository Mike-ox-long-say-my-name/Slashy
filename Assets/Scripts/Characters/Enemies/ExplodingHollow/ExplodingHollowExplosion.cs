using Core.Attacking;
using Core.Characters.Interfaces;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowExplosion : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.VelocityMovement.Stop();

            Context.Animator.SetTrigger("explode");

            Context.ExplosionAttack.StartAttack(_ => SwitchState<ExplodingHollowDeath>());
        }

        public override void OnHitReceived(HitInfo info)
        {
        }

        public override void OnStaggered(HitInfo info)
        {
        }

        public override void OnDeath(HitInfo info)
        {
            Context.ExplosionAttack.InterruptAttack();
            base.OnDeath(info);
        }
    }
}