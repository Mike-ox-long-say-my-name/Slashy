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

            Context.CanDash.Lock(this);
            Context.CanJump.Lock(this);
            Context.CanAttack.Lock(this);

            void AttackEnded1(bool interrupted)
            {
                if (Context.IsLightAttackPressed.CheckAndReset())
                {
                    Context.ResetBufferedInput();
                    if (!Context.PlayerCharacter.HasStamina)
                    {
                        AttackEnded2(interrupted);
                        return;
                    }

                    Context.PlayerCharacter.SpendStamina(Context.ActionConfig.LightAttack2StaminaCost);
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
                SwitchState(Factory.Idle());
                Context.StartCoroutine(RecoveryRoutine());

                IEnumerator RecoveryRoutine()
                {
                    // Иначе ломаются анимации
                    yield return new WaitForSeconds(Context.AttackRecoveryTime);

                    Context.CanDash.TryUnlock(this);
                    Context.CanJump.TryUnlock(this);
                    Context.CanAttack.TryUnlock(this);
                }
            }

            Context.PlayerCharacter.SpendStamina(Context.ActionConfig.LightAttack1StaminaCost);
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