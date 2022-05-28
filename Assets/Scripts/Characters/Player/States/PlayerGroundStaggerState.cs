using Core.Characters.Interfaces;

namespace Characters.Player.States
{
    public class PlayerGroundStaggerState : PlayerBaseGroundedState
    {
        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            Context.Animator.StartStaggerAnimation();
        }

        public override void OnStaggerEnded()
        {
            SwitchState<PlayerGroundedState>();
        }

        public override void ExitState()
        {
            Context.Animator.EndStaggerAnimation();
        }
    }
}