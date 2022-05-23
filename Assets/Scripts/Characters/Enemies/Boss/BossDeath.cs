namespace Characters.Enemies.Boss
{
    public class BossDeath : BossBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetTrigger("death");
            Context.BossEvents.Died?.Invoke();
            Context.Destroyable.DestroyLater();
        }

        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
        }
    }
}