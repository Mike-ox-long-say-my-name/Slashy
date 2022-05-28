namespace Characters.Enemies.Boss
{
    public class BossDeath : BossBaseState
    {
        public override void EnterState()
        {
            Context.Animator.PlayDeathAnimation();
            Context.BossEvents.Died?.Invoke();
            Context.DestroyHelper.DestroyLater();
        }

        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
        }
    }
}