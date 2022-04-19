using System.Collections;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerStaggerState : PlayerBaseState
    {
        public PlayerStaggerState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            // TODO: добавить аниме цию
            // Context.AnimatorComponent.SetTrigger("stagger");

            IEnumerator RecoverRoutine(float recoverTime)
            {
                yield return new WaitForSeconds(recoverTime);

                SwitchState(Factory.Idle());
            }

            Context.StartCoroutine(RecoverRoutine(Context.ActionConfig.StaggerRecoveryTime));
        }
    }
}