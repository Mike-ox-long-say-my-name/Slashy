using System.Collections;
using UnityEngine;

namespace Player.States
{
    public class PlayerDashState : PlayerBaseState
    {
        public PlayerDashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            Context.Animator.SetTrigger("dash");
            Dash(new Vector3(Context.MoveInput.x, 0, Context.MoveInput.y));
        }

        public override void UpdateStates()
        {
        }

        public override void UpdateState()
        {
        }

        public override void ExitState()
        {
        }

        private void Dash(Vector3 direction)
        {
            Context.CanDash = false;
            Context.CanJump = false;

            Context.AppliedVelocityX = 0;
            Context.AppliedVelocityZ = 0;

            var fullMove = direction * Context.DashDistance;

            IEnumerator DashCoroutine()
            {
                var passedTime = 0f;
                var lastMoveTime = Time.time;
                while (passedTime < Context.DashTime)
                {
                    var timeStep = Time.time - lastMoveTime;
                    passedTime += timeStep;
                    lastMoveTime = Time.time;

                    var fraction = passedTime / Context.DashTime;

                    if (!Context.IsInvincible && fraction >= Context.ActionConfig.DashInvincibilityStart &&
                        fraction < Context.ActionConfig.DashInvincibilityEnd)
                    {
                        EnableInvincibility();
                    }
                    else if (Context.IsInvincible && fraction >= Context.ActionConfig.DashInvincibilityEnd)
                    {
                        DisableInvincibility();
                    }
                    
                    Context.CharacterController.Move(fullMove * timeStep);
                    yield return new WaitForEndOfFrame();
                }

                Context.AppliedVelocityX = 0;
                Context.AppliedVelocityZ = 0;

                Context.StartCoroutine(RecoverFromDashRoutine(Context.DashRecovery));

                SwitchState(Factory.Grounded());
            }

            Context.StartCoroutine(DashCoroutine());
        }

        private IEnumerator RecoverFromDashRoutine(float recoverTime)
        {
            // Иначе ломается анимация при моментальном прыжке после даша
            yield return new WaitForEndOfFrame();
            Context.CanJump = true;

            yield return new WaitForSeconds(recoverTime);
            Context.CanDash = true;
        }

        private void EnableInvincibility()
        {
            Context.IsInvincible = true;
            if (Context.Hurtbox)
            {
                Context.Hurtbox.Disable();
            }
        }

        private void DisableInvincibility()
        {
            Context.IsInvincible = false;
            if (Context.Hurtbox)
            {
                Context.Hurtbox.Enable();
            }
        }
    }
}