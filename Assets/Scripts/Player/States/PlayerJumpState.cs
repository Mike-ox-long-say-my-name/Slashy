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
            Context.Animator.SetTrigger("jump");
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
            else if (Context.IsLightAttackPressed.CheckAndReset())
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

            // TODO: добавить атаку в прыжке
            // SetSubState(Factory.Attack());
            // SubState.EnterState();
        }
    }
}