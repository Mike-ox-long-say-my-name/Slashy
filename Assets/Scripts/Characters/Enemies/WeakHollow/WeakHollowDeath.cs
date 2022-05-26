using Core;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowDeath : WeakHollowBaseState
    {
        public override void EnterState()
        {
            AggroListener.Instance.DecreaseAggroCounter();
            Context.AnimatorComponent.SetTrigger("death");

            Context.Destroyable.DestroyLater();
        }

        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
        }
    }
}