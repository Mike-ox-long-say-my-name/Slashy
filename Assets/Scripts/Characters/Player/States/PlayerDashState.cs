using System.Collections;
using Core.Characters;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerDashState : PlayerBaseGroundedState

    {
        private Coroutine _dashRoutine;

        public PlayerDashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-dashing", true);

            Context.PlayerCharacter.SpendStamina(Context.PlayerConfig.DashStaminaCost);
            var direction = new Vector3(Context.Input.MoveInput.x, 0, Context.Input.MoveInput.y);
            Dash(direction);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-dashing", false);

            Context.Movement.ResetXZVelocity();
            Context.StartCoroutine(RecoverFromDashRoutine(Context.PlayerConfig.DashRecovery));
        }

        public override void InterruptState(StateInterruption interruption)
        {
            Context.StopCoroutine(_dashRoutine);
            base.InterruptState(interruption);
        }

        private void Dash(Vector3 direction)
        {
            Context.Movement.ResetXZVelocity();

            IEnumerator DashCoroutine(CharacterMovement movement, float dashTime, Vector3 targetMove)
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
                    movement.MoveRaw(move);
                    yield return null;
                }

                SwitchState(Factory.Grounded());
            }

            var fullMove = direction * Context.PlayerConfig.DashDistance;
            _dashRoutine = Context.StartCoroutine(DashCoroutine(Context.Movement, Context.PlayerConfig.DashTime, fullMove));
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
            if (!Context.IsInvincible && fraction >= Context.PlayerConfig.DashInvincibilityStart &&
                fraction < Context.PlayerConfig.DashInvincibilityEnd)
            {
                EnableInvincibility();
            }
            else if (Context.IsInvincible && fraction >= Context.PlayerConfig.DashInvincibilityEnd)
            {
                DisableInvincibility();
            }
        }

        private IEnumerator RecoverFromDashRoutine(float recoverTime)
        {
            Context.CanDash.Lock(this);

            yield return new WaitForSeconds(recoverTime);
            Context.CanDash.TryUnlock(this);
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