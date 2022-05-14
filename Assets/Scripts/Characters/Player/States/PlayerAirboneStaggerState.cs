using System.Collections;
using Core.Attacking;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerAirboneStaggerState : PlayerAirboneState
    {
        private Coroutine _recoverFromStaggerFallRoutine;

        public override void OnStaggered(HitInfo info)
        {
        }

        public override void OnDeath(HitInfo info)
        {
            if (_recoverFromStaggerFallRoutine != null)
            {
                Context.StopCoroutine(_recoverFromStaggerFallRoutine);
            }
            base.OnDeath(info);
        }

        public override void EnterState()
        {
            // TODO: добавить анимацию стана в полете
            // Переход делать через учет значения is-airbone
            Context.Animator.SetBool("is-staggered", true);

            Context.Input.ResetBufferedInput();
        }

        public override void UpdateState()
        {
            CheckStateSwitch();
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-staggered", false);
        }

        private void CheckStateSwitch()
        {
            if (Context.VelocityMovement.BaseMovement.IsGrounded && _recoverFromStaggerFallRoutine == null)
            {
                Context.VelocityMovement.Stop();
                _recoverFromStaggerFallRoutine =
                    Context.StartCoroutine(RecoverFromStaggerFall(Context.PlayerConfig.StaggerFallTime));
            }
        }

        private IEnumerator RecoverFromStaggerFall(float recoveryTime)
        {
            yield return new WaitForSeconds(recoveryTime);

            SwitchState<PlayerGroundedState>();
        }
    }
}