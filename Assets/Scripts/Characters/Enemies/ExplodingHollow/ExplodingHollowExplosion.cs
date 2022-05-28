using Core.Attacking;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowExplosion : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.Animator.PlayExplodeAnimation();
            Context.ExplosionAttack.StartAttack(_ => Context.Character.Kill());
        }

        public override void OnHitReceived(HitInfo info)
        {
        }

        public override void OnStaggered(HitInfo info)
        {
        }

        public override void OnDeath(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            base.OnDeath(info);
        }
    }
}