using Characters.Enemies.States;
using Core.Characters;

namespace Characters.Enemies.Rogue
{
    public class RogueDeath : EnemyBaseState<RogueStateMachine>
    {
        public override void EnterState()
        {
            Context.Deaggro();
            Context.AutoMovement.ResetState();
            Context.AutoMovement.UnlockRotation();

            Context.Hurtbox.Disable();
            Context.Animator.PlayDeathAnimation();
            Context.DestroyHelper.DestroyLater();
        }

        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
        }
    }
}