using UnityEngine;

namespace Player.States
{
    public class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Debug.Log("Enter Walk");
        }

        public override void UpdateState()
        {
            Context.AppliedVelocityX = Context.HorizontalMoveSpeed * Context.MoveInput.x;
            Context.AppliedVelocityZ = Context.VerticalMoveSpeed * Context.MoveInput.y;

            CheckStateSwitch();
        }

        public override void ExitState()
        {
            Debug.Log("Exit Walk");
        }

        private void CheckStateSwitch()
        {
            if (Context.MoveInput.sqrMagnitude < 1e-8)
            {
                SwitchState(Factory.Idle());
            }
        }
    }
}