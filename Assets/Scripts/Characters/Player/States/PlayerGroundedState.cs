using Core;
using Core.Levels;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerGroundedState : PlayerBaseGroundedState
    {
        public override void EnterState()
        {
            Context.AttackedThisAirTime = false;
        }

        public override void UpdateState()
        {
            ApplyMoveInput();
            SetWalkAnimationBasedOnInput();

            CheckStateSwitch();
        }

        private void SetWalkAnimationBasedOnInput()
        {
            var isWalking = Context.Input.MoveInput.sqrMagnitude > 0;
            var animator = Context.Animator;
            if (isWalking)
            {
                animator.StartWalkAnimation();
            }
            else
            {
                animator.EndWalkAnimation();
            }
        }

        public override void OnInteracted()
        {
            var result = Context.Interactor.TryInteract(InteractionMask.Any);
            if (result.Type != InteractionType.TouchedBonfire)
            {
                return;
            }

            Context.BonfireToTouch = (Bonfire)result.Sender;
            SwitchState<PlayerMovingToFromExternalEvent>();
        }

        public override void OnWarpStarted(Vector3 target)
        {
            base.OnWarpStarted(target);
            SwitchState<PlayerMovingToFromExternalEvent>();
        }

        public override void OnWarpEnded(Vector3 target)
        {
            base.OnWarpEnded(target);
            SwitchState<PlayerMovingToFromExternalEvent>();
        }

        private void CheckStateSwitch()
        {
            if (!Context.VelocityMovement.BaseMovement.IsGrounded)
            {
                SwitchState<PlayerFallState>();
            }
            else if (Context.ShouldDash)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerDashState>();
            }
            else if (Context.ShouldJump)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerJumpState>();
            }
            else if (Context.ShouldHeal)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerHealState>();
            }
            else if (Context.ShouldLightAttack)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerGroundLightAttackState>();
            }
            else if (Context.ShouldStrongAttack)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerGroundStrongAttackState>();
            }
        }
    }
}