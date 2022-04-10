using UnityEngine;

namespace Player.States
{
    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            Debug.Log("Enter Fall");
        }

        public override void UpdateState()
        {
            Context.ApplyGravity();
            Context.ApplyAirboneMovement();

            CheckStateSwitch();
        }

        public override void ExitState()
        {
            Debug.Log("Exit Fall");
        }

        private void CheckStateSwitch()
        {
            if (Context.CharacterController.isGrounded)
            {
                SwitchState(Factory.Grounded());
            }
        }
    }
}