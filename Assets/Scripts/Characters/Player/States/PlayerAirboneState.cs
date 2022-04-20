using System;

namespace Characters.Player.States
{
    public abstract class PlayerAirboneState : PlayerBaseState
    {
        protected PlayerAirboneState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-airbone", true);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-airbone", false);
        }

        protected void HandleGravity()
        {
            Context.Movement.ApplyGravity();
        }

        protected void HandleAirboneControl()
        {
            Context.Movement.Move(Context.Input.MoveInput);
        }

        public override void InterruptState(StateInterruption interruption)
        {
            switch (interruption)
            {
                case StateInterruption.Staggered:
                    SwitchState(Factory.AirboneStagger());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(interruption), interruption,
                        $"Invalid interruption for state {this}");
            }
        }
    }
}