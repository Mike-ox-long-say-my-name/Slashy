using Core.Attacking;
using Core.Characters.Interfaces;
using System.Collections;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerDashState : PlayerBaseGroundedState
    {
        private Coroutine _dashRoutine;

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            Context.Animator.StartDashAnimation();
            Context.DashAudioSource.Play();

            Context.ResourceSpender.SpendFor(PlayerResourceAction.Dash);
            DashTowardsInputDirection();
        }

        private void DashTowardsInputDirection()
        {
            var input = Context.Input.MoveInput;
            Context.VelocityMovement.BaseMovement.Rotate(input.x);
            var direction = new Vector3(input.x, 0, input.y);
            Dash(direction);
        }

        public override void ExitState()
        {
            Context.Animator.EndDashAnimation();
            Context.StartCoroutine(RecoverFromDashRoutine(Context.PlayerConfig.DashRecovery));
        }

        public override void OnDeath(HitInfo info)
        {
            Context.StopCoroutine(_dashRoutine);
            base.OnDeath(info);
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.StopCoroutine(_dashRoutine);
            base.OnStaggered(info);
        }

        private void Dash(Vector3 direction)
        {
            IEnumerator DashCoroutine(IBaseMovement movement, float dashTime, Vector3 targetMove)
            {
                var passedTime = 0f;
                while (passedTime < dashTime)
                {
                    var timeStep = Time.deltaTime;
                    passedTime += timeStep;

                    var fraction = passedTime / dashTime;

                    ApplyInvincibilityLogic(fraction);
                    TickDashEffectController(timeStep);

                    var move = targetMove * (timeStep / dashTime);
                    movement.Move(move);
                    yield return null;
                }

                SwitchState<PlayerGroundedState>();
            }

            var fullMove = direction * Context.PlayerConfig.DashDistance;
            _dashRoutine = Context.StartCoroutine(
                DashCoroutine(Context.VelocityMovement.BaseMovement, Context.PlayerConfig.DashTime, fullMove));
        }

        private void TickDashEffectController(float timeStep)
        {
            Context.DashEffectController.Tick(Context.transform, Context.SpriteRenderer.sprite, timeStep);
        }

        private void ApplyInvincibilityLogic(float fraction)
        {
            bool ShouldDisableInvincibility()
            {
                return Context.IsInvincible && fraction >= Context.PlayerConfig.DashInvincibilityEnd;
            }

            bool ShouldEnableInvincibility()
            {
                return !Context.IsInvincible && fraction >= Context.PlayerConfig.DashInvincibilityStart &&
                       fraction < Context.PlayerConfig.DashInvincibilityEnd;
            }

            if (ShouldEnableInvincibility())
            {
                EnableInvincibility();
            }
            else if (ShouldDisableInvincibility())
            {
                DisableInvincibility();
            }
        }

        private IEnumerator RecoverFromDashRoutine(float recoverTime)
        {
            Context.DashRecoveryLock.Lock(this);

            yield return new WaitForSeconds(recoverTime);
            Context.DashRecoveryLock.TryUnlock(this);
        }

        private void EnableInvincibility()
        {
            Context.IsInvincible = true;
            Context.Hurtbox.Disable();
        }

        private void DisableInvincibility()
        {
            Context.IsInvincible = false;
            Context.Hurtbox.Enable();
        }
    }
}