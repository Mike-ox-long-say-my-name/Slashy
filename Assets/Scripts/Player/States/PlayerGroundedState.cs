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
            Debug.Log("Enter Grounded");
        }

        public override void UpdateState()
        {
            Context.ApplyGravity();

            CheckStateSwitch();
        }

        public override void ExitState()
        {
            Debug.Log("Exit Grounded");
        }

        private void CheckStateSwitch()
        {
            if (Context.IsDashPressed)
            {
                SwitchState(Factory.Dashing());
            }
            else if (Context.IsJumpPressed)
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