using UnityEngine;

namespace Player.States
{
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
            IsRootState = true;
            SetSubState(Context.MoveInput.sqrMagnitude > 0 ? Factory.Walk() : Factory.Idle());
        }

        public override void EnterState()
        {
            if (SubState.GetType() == typeof(PlayerWalkState))
            {
                Context.Animator.SetTrigger("walk");
            }
            else if (SubState.GetType() == typeof(PlayerIdleState))
            {
                Context.Animator.SetTrigger("idle");
            }
        }

        public override void UpdateState()
        {
            Context.ApplyGravity();

            CheckStateSwitch();
        }

        public override void ExitState()
        {
        }

        private void CheckStateSwitch()
        {
            if (Context.IsDashPressed && Context.CanDash)
            {
                SwitchState(Factory.Dashing());
            }
            else if (Context.IsJumpPressed && Context.CanJump)
            {
                SwitchState(Factory.Jump());
            }
            else if (!Context.CharacterController.isGrounded)
            {
                SwitchState(Factory.Fall());
            }
        }
    }
}