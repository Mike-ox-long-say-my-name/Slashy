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
            SubState.EnterState();
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
            //if (Context.IsAttacking)
            //{
            //    return;
            //}

            if (Context.CanDash && Context.IsDashPressed.CheckAndReset())
            {
                Context.ResetBufferedInput();
                SwitchState(Factory.Dash());
            }
            else if (Context.CanJump && Context.IsJumpPressed.CheckAndReset())
            {
                Context.ResetBufferedInput();
                SwitchState(Factory.Jump());
            }
            else if (!Context.CharacterController.isGrounded)
            {
                Debug.Log("fall");
                SwitchState(Factory.Fall());
            }
            else if (Context.CanAttack && Context.IsLightAttackPressed.CheckAndReset())
            {
                Context.ResetBufferedInput();
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (Context.IsAttacking)
            {
                return;
            }

            SetSubState(Factory.Attack());
            SubState.EnterState();
        }
    }
}