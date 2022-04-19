using System.Collections;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerDashState : PlayerBaseState
    {
        public PlayerDashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            Context.Player.SpendStamina(Context.ActionConfig.DashStaminaCost);
            Dash(new Vector3(Context.MoveInput.x, 0, Context.MoveInput.y));
            Context.AnimatorComponent.SetTrigger("dash");
        }

        public override void UpdateStates()
        {
            UpdateState();
        }

        private void Dash(Vector3 direction)
        {
            Context.CanDash.Lock(this);
            Context.CanJump.Lock(this);
            Context.CanAttack.Lock(this);
            Context.CanRotate.Lock(this);

            Context.Movement.ResetXZVelocity();

            IEnumerator DashCoroutine(PlayerMovement movement, float dashTime, Vector3 targetMove, float recovery)
            {
                var passedTime = 0f;
                while (passedTime < dashTime)
                {
                    var timeStep = Time.deltaTime;
                    passedTime += timeStep;

                    var fraction = passedTime / dashTime;

                    ApplyInvincibilityLogic(fraction);
                    TickDashEffectController(timeStep);

                    movement.MoveRaw(targetMove * timeStep);
                    yield return null;
                }

                movement.ResetXZVelocity();
                SwitchState(Factory.Grounded());
                Context.StartCoroutine(RecoverFromDashRoutine(recovery));
            }
            
            var fullMove = direction * Context.DashDistance;
            Context.StartCoroutine(DashCoroutine(Context.Movement, Context.DashTime, fullMove, Context.DashRecovery));
        }

        private void TickDashEffectController(float timeStep)
        {
            if (Context.HasDashEffectController && Context.HasSpriteRenderer)
            {
                Context.DashEffectController.Tick(Context.transform, Context.SpriteRenderer.sprite, timeStep);
            }
        }

        private void ApplyInvincibilityLogic(float fraction)
        {
            if (!Context.IsInvincible && fraction >= Context.ActionConfig.DashInvincibilityStart &&
                fraction < Context.ActionConfig.DashInvincibilityEnd)
            {
                EnableInvincibility();
            }
            else if (Context.IsInvincible && fraction >= Context.ActionConfig.DashInvincibilityEnd)
            {
                DisableInvincibility();
            }
        }

        private IEnumerator RecoverFromDashRoutine(float recoverTime)
        {
            Context.CanRotate.TryUnlock(this);

            yield return new WaitForEndOfFrame();
            Context.CanJump.TryUnlock(this);
            Context.CanAttack.TryUnlock(this);
            Context.CanDash.ReleaseOwnership(this);

            yield return new WaitForSeconds(recoverTime);
            Context.CanDash.TryUnlock();
        }

        private void EnableInvincibility()
        {
            Context.IsInvincible = true;
            if (Context.HasHurtbox)
            {
                Context.HurtboxComponent.Disable();
            }
        }

        private void DisableInvincibility()
        {
            Context.IsInvincible = false;
            if (Context.HasHurtbox)
            {
                Context.HurtboxComponent.Enable();
            }
        }
    }
}