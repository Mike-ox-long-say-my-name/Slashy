using System;

namespace Characters.Player.States
{
    public abstract class PlayerBaseGroundedState : PlayerBaseState
    {
        protected PlayerBaseGroundedState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        protected void HandleGravity()
        {
            Context.Movement.ApplyGravity();
        }

        public override void InterruptState(StateInterruption interruption)
        {
            switch (interruption)
            {
                case StateInterruption.Staggered:
                    SwitchState(Factory.GroundStagger());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interruption), interruption,
                        $"Invalid interruption for state {GetType().Name}");
            }
        }
    }
}