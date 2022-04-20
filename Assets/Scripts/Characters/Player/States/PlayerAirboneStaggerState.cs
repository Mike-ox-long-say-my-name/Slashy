using System.Collections;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerAirboneStaggerState : PlayerAirboneState
    {
        private Coroutine _recoverFromStaggerFallRoutine;

        public PlayerAirboneStaggerState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            // TODO: добавить анимацию стана в полете
            // Переход делать через учет значения is-airbone
            Context.AnimatorComponent.SetBool("is-staggered", true);

            Context.Input.ResetBufferedInput();
        }

        public override void UpdateState()
        {
            HandleGravity();

            CheckStateSwitch();
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-staggered", false);
        }

        private void CheckStateSwitch()
        {
            if (Context.Movement.IsGrounded && _recoverFromStaggerFallRoutine == null)
            {
                Context.Movement.ResetXZVelocity();
                _recoverFromStaggerFallRoutine =
                    Context.StartCoroutine(RecoverFromStaggerFall(Context.PlayerConfig.StaggerFallTime));
            }
        }

        private IEnumerator RecoverFromStaggerFall(float recoveryTime)
        {
            yield return new WaitForSeconds(recoveryTime);

            SwitchState(Factory.Grounded());
        }
    }
}