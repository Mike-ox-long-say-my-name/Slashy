using Core.Characters.Interfaces;

namespace Characters.Player.States
{
    public class PlayerGroundStaggerState : PlayerBaseGroundedState
    {
        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            Context.AnimatorComponent.SetBool("is-staggered", true);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-staggered", false);
        }
    }
}