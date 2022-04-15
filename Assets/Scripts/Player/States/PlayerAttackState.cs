using System.Collections;
using UnityEngine;

namespace Player.States
{
    public class PlayerAttackState : PlayerBaseState
    {
        public PlayerAttackState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Context.AppliedVelocityX = 0;
            Context.AppliedVelocityZ = 0;
            Context.CanDash = false;
            Context.CanJump = false;
            Context.CanAttack = false;

            void AttackEnded1(bool interrupted)
            {
                if (Context.IsLightAttackPressed.CheckAndReset())
                {
                    Context.Animator.SetTrigger("attack-long");
                    Context.LightAttackExecutor2.StartExecution(Context.PlayerCharacter, AttackEnded2);
                }
                else
                {
                    AttackEnded2(interrupted);
                }
            }

            void AttackEnded2(bool _)
            {
                Context.CanDash = true;
                Context.CanJump = true;
                Context.CanAttack = true;
                SwitchState(Factory.Idle());
            }
            
            Context.Animator.ResetTrigger("attack-long");
            Context.Animator.SetTrigger("attack-short");
            Context.LightAttackExecutor1.StartExecution(Context.PlayerCharacter, AttackEnded1);
        }

        public override void UpdateState()
        {
        }

        public override void ExitState()
        {
        }
    }
}