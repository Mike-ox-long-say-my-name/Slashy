using Characters.Enemies.States;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowDeath : EnemyBaseState<ExplodingHollowStateMachine>
    {
        public override void EnterState()
        {
            Context.Hurtbox.Disable();
            // Context.VelocityMovement.Stop();
            Context.Animator.SetTrigger("death");

            // Почти не захардкожено
            Context.Destroyable.DestroyAfter(0.3f);
        }
    }
}