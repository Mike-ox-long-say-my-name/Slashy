using Core.Attacking;
using Core.Characters.Interfaces;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowAttack : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            Context.Animator.SetTrigger("attack");

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
            if (Context.PunchAttack.IsAttacking)
            {
                Context.PunchAttack.InterruptAttack();
            }
            base.OnDeath(info);
        }

        public override void OnStaggered(HitInfo info)
        {
            if (Context.PunchAttack.IsAttacking)
            {
                Context.PunchAttack.InterruptAttack();
            }
            base.OnStaggered(info);
        }

        public override void OnHitReceived(HitInfo info)
        {
            if (info.Source.Character.Team == Team.Player && Context.PunchAttack.IsAttacking)
            {
                Context.PunchAttack.InterruptAttack();
            }
            base.OnHitReceived(info);
        }
    }
}