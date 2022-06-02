using Characters.Enemies.States;
using Core.Characters;

namespace Characters.Enemies.Rogue
{
    public class RogueDeath : EnemyBaseState<RogueStateMachine>
    {
        public override void EnterState()
        {
            Context.AutoMovement.ResetState();
            Context.AutoMovement.UnlockRotation();

            Context.Hurtbox.Disable();
            HackAnimations();
            Context.Animator.PlayDeathAnimation();
            Context.DestroyHelper.DestroyLater();
        }

        private void HackAnimations()
        {
            Context.Animator.EndJumpAtAnimation();
            Context.Animator.EndJumpAwayAnimation();
            Context.Animator.EndAirboneAnimation();
            Context.Animator.EndStaggerAnimation();
            Context.Animator.EndWalkAnimation();
        }

        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
        }
    }
}