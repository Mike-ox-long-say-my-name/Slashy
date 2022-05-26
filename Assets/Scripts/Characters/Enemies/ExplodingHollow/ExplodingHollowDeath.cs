using Characters.Enemies.States;
using Core;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowDeath : EnemyBaseState<ExplodingHollowStateMachine>
    {
        public override void EnterState()
        {
            AggroListener.Instance.DecreaseAggroCounter();

            Context.Hurtbox.Disable();
            Context.Animator.SetTrigger("death");
            Context.Destroyable.DestroyLater();
        }

        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
        }
    }
}