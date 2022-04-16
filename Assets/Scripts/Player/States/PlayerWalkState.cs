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
            Context.Animator.SetTrigger("walk");
        }

        public override void UpdateState()
        {
            Context.AppliedVelocityX = Context.HorizontalMoveSpeed * Context.MoveInput.x;
            Context.AppliedVelocityZ = Context.VerticalMoveSpeed * Context.MoveInput.y;

            CheckStateSwitch();
        }

        public override void ExitState()
        {
        }

        private void CheckStateSwitch()
        {
            if (Mathf.Approximately(Context.MoveInput.sqrMagnitude, 0))
            {
                SwitchState(Factory.Idle());
            }
        }
    }
}