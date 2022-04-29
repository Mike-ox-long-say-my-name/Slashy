using System.Collections;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerGroundStaggerState : PlayerBaseGroundedState
    {
        public override void EnterState()
        {
            Context.VelocityMovement.Stop();

            Context.AnimatorComponent.SetBool("is-staggered", true);

            Context.Input.ResetBufferedInput();
            Context.StartCoroutine(RecoverRoutine(Context.LastHitInfo.StaggerTime));
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-staggered", false);
        }

        private IEnumerator RecoverRoutine(float staggerTime)
        {
            yield return new WaitForSeconds(staggerTime);

            SwitchState<PlayerGroundedState>();
        }
    }
}