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
            Context.Movement.ResetXZVelocity();

            Context.CanDash.Lock(this);
            Context.CanJump.Lock(this);
            Context.CanAttack.Lock(this);

            void AttackEndedFirst(bool interrupted)
            {
                if (Context.IsLightAttackPressed.CheckAndReset())
                {
                    Context.ResetBufferedInput();
                    if (!Context.Player.HasStamina)
                    {
                        AttackEndedSecond(interrupted);
                        return;
                    }

                    Context.Player.SpendStamina(Context.ActionConfig.LightAttackSecondStaminaCost);
                    Context.AnimatorComponent.SetTrigger("attack-long");
                    Context.LightAttackSecond.StartExecution(Context.Player, AttackEndedSecond);
                }
                else
                {
                    AttackEndedSecond(interrupted);
                }
            }

            void AttackEndedSecond(bool _)
            {
                SwitchState(Factory.Idle());
                Context.StartCoroutine(RecoveryRoutine());

                IEnumerator RecoveryRoutine()
                {
                    // Иначе ломаются анимации
                    yield return new WaitForSeconds(Context.ActionConfig.LightAttackRecovery);

                    Context.CanDash.TryUnlock(this);
                    Context.CanJump.TryUnlock(this);
                    Context.CanAttack.TryUnlock(this);
                }
            }

            Context.Player.SpendStamina(Context.ActionConfig.LightAttackFirstStaminaCost);
            Context.AnimatorComponent.ResetTrigger("attack-long");
            Context.AnimatorComponent.SetTrigger("attack-short");
            Context.LightAttackFirst.StartExecution(Context.Player, AttackEndedFirst);
        }
    }
}