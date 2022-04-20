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
            Context.AnimatorComponent.SetTrigger("hurt");

            Context.CanAttack.Lock(this);
            Context.CanDash.Lock(this);
            Context.CanJump.Lock(this);

            Context.ResetBufferedInput();
            if (Context.Movement.IsGrounded) 
            {
                //TODO: move back
                Context.Movement.ResetXZVelocity();
            }

            IEnumerator RecoverRoutine(float staggerTime, float recoverTime)
            {
                yield return new WaitForSeconds(staggerTime);
                
                SwitchState(Factory.Idle());
                
                yield return new WaitForSeconds(recoverTime);
                Context.CanAttack.TryUnlock(this);
                Context.CanDash.TryUnlock(this);
                Context.CanJump.TryUnlock(this);
            }

            Context.StartCoroutine(RecoverRoutine(Context.ActionConfig.StaggerTime, Context.ActionConfig.StaggerRecoveryTime));
        }
    }
}