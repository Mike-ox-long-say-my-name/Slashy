namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowDeath : WeakHollowBaseState
    {
        public override void EnterState()
        {
            Context.Animator.PlayDeathAnimation();
            Context.DestroyHelper.DestroyLater();
        }

        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
        }
    }
}