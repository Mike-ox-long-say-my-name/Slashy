using Core.Attacking;
using Core.Characters.Interfaces;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowAttack : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.Animator.PlayPunchAnimation();

            Context.PunchAttack.StartAttack(result =>
            {
                if (result.WasCompleted)
                {
                    SwitchState<ExplodingHollowPursue>();
                }
            });
        }

        public override void OnDeath(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            base.OnDeath(info);
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            base.OnStaggered(info);
        }

        public override void OnHitReceived(HitInfo info)
        {
            var source = info.Source;
            if (source.Character is { Team: Team.Player } && Context.PunchAttack.IsAttacking)
            {
                Context.AttackExecutorHelper.InterruptAllRunning();
            }

            base.OnHitReceived(info);
        }
    }
}