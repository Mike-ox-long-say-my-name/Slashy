using Core;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowDeath : WeakHollowBaseState
    {
        public override void EnterState()
        {
            BorderManager.Instance.DecreaseAggroCounter();
            Context.AnimatorComponent.SetTrigger("death");

            Context.Destroyable.DestroyLater();
        }
    }
}