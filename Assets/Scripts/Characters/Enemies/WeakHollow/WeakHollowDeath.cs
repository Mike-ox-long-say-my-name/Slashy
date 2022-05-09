namespace Characters.Enemies
{
    public class WeakHollowDeath : WeakHollowBaseState
    {
        public override void EnterState()
        {
            Context.AnimatorComponent.SetTrigger("death");

            Context.Destroyable.DestroyLater();
        }
    }
}