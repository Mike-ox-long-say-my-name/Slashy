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
            Context.Animator.StartStaggerAnimation();
            Context.Input.ResetBufferedInput();
        }

        public override void UpdateState()
        {
            CheckStateSwitch();
        }

        public override void ExitState()
        {
            Context.Animator.EndStaggerAnimation();
        }

        private void CheckStateSwitch()
        {
            if (!DidLand())
            {
                return;
            }
            Context.VelocityMovement.Stop();
            StartRecovery();
        }

        private void StartRecovery()
        {
            _recoverFromStaggerFallRoutine =
                Context.StartCoroutine(RecoverFromStaggerFall(Context.PlayerConfig.StaggerFallTime));
        }

        private bool DidLand()
        {
            return Context.VelocityMovement.BaseMovement.IsGrounded && _recoverFromStaggerFallRoutine == null;
        }

        private IEnumerator RecoverFromStaggerFall(float recoveryTime)
        {
            yield return new WaitForSeconds(recoveryTime);

            SwitchState<PlayerGroundedState>();
        }
    }
}