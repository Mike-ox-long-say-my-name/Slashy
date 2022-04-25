using Core.Attacking;
using Core.Characters.Interfaces;
using System.Collections;
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
            Context.Movement.Stop();

            Context.AnimatorComponent.SetBool("is-dashing", true);

            Context.Character.SpendStamina(Context.PlayerConfig.DashStaminaCost);
            var direction = new Vector3(Context.Input.MoveInput.x, 0, Context.Input.MoveInput.y);
            Dash(direction);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-dashing", false);
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
            IEnumerator DashCoroutine(IRawMovement movement, float dashTime, Vector3 targetMove)
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
            Context.Hurtbox.Disable();
        }

        private void DisableInvincibility()
        {
            Context.IsInvincible = false;
            Context.Hurtbox.Enable();
        }
    }
}