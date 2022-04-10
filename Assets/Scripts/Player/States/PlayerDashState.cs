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
            Debug.Log("Enter Dash");
        
            Dash(new Vector3(Context.MoveInput.x, 0, Context.MoveInput.y));
        }

        public override void UpdateState()
        {
        }

        public override void ExitState()
        {
            Debug.Log("Exit Dash");
        }

        private void Dash(Vector3 direction)
        {
            var step = direction * (Context.DashDistance / Context.DashTime);

            IEnumerator DashCoroutine()
            {
                var passedTime = 0f;
                while (passedTime < Context.DashTime)
                {
                    passedTime += Time.deltaTime;
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

                    Context.CharacterController.Move(Time.deltaTime * step);
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(Context.DashRecovery);
                SwitchState(Factory.Grounded());
            }

            Context.StartCoroutine(DashCoroutine());
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