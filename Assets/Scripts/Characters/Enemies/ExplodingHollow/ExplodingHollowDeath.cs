using Characters.Enemies.States;
using Core;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowDeath : EnemyBaseState<ExplodingHollowStateMachine>
    {
        public override void EnterState()
        {
            Context.Deaggro();
            Context.Hurtbox.Disable();
            Context.Animator.PlayDeathAnimation();
            Context.DestroyHelper.DestroyLater();
        }

        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
        }
    }
}