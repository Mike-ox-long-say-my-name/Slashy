using System;
using Core.Characters;

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

        public override void InterruptState(CharacterInterruption interruption)
        {
            switch (interruption.Type)
            {
                case CharacterInterruptionType.Staggered:
                    SwitchState(Factory.GroundStagger());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interruption), interruption,
                        $"Invalid interruption for state {GetType().Name}");
            }
        }
    }
}