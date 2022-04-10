using UnityEngine;

namespace Player.States
{
    public class PlayerJumpState : PlayerBaseState
    {

        public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            Debug.Log("Enter Jump");

            Context.AppliedVelocityY = Context.JumpStartVelocity;
        }

        public override void UpdateState()
        {
            Context.ApplyGravity();
            Context.ApplyAirboneMovement();

            CheckStateSwitch();
        }

        public override void ExitState()
        {
            Debug.Log("Exit Jump");
        }

        public virtual void CheckStateSwitch()
        {
            if (Context.CharacterController.isGrounded)
            {
                SwitchState(Factory.Grounded());
            }
            else if (Context.AppliedVelocityY < 0)
            {
                SwitchState(Factory.Fall());
            }
        }
    }
}