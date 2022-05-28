using Characters.Enemies.States;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowDeath : EnemyBaseState<ExplodingHollowStateMachine>
    {
        public override void EnterState()
        {
            Context.Hurtbox.Disable();
            Context.Animator.PlayDeathAnimation();
            Context.DestroyHelper.DestroyLater();
        }

        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
        }
    }
}