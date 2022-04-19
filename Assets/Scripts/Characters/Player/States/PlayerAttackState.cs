using System.Collections;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerAttackState : PlayerBaseState
    {
        public PlayerAttackState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        private int _currentAttack;

        public override void EnterState()
        {
            _currentAttack = 0;

            Context.Movement.ResetXZVelocity();

            Context.CanDash.Lock(this);
            Context.CanJump.Lock(this);
            Context.CanAttack.Lock(this);

            void AttackEndedFirst(bool interrupted)
            {
                if (!interrupted && Context.IsLightAttackPressed.CheckAndReset())
                {
                    Context.ResetBufferedInput();
                    if (!Context.Player.HasStamina)
                    {
                        AttackEndedSecond(false);
                        return;
                    }
                    
                    _currentAttack = 2;
                    Context.Player.SpendStamina(Context.ActionConfig.LightAttackSecondStaminaCost);
                    Context.AnimatorComponent.SetTrigger("attack-long");
                    Context.LightAttackSecond.StartExecution(Context.Player, AttackEndedSecond);
                }
                else
                {
                    AttackEndedSecond(interrupted);
                }
            }

            void AttackEndedSecond(bool interrupted)
            {
                if (!interrupted)
                {
                    SwitchState(Factory.Idle());
                    Context.StartCoroutine(RecoveryRoutine());
                }
                else
                {
                    UnlockActions();
                }

                IEnumerator RecoveryRoutine()
                {
                    // Иначе ломаются анимации
                    yield return new WaitForSeconds(Context.ActionConfig.LightAttackRecovery);
                    UnlockActions();
                }
            }

            Context.Player.SpendStamina(Context.ActionConfig.LightAttackFirstStaminaCost);
            Context.AnimatorComponent.ResetTrigger("attack-long");
            Context.AnimatorComponent.SetTrigger("attack-short");

            _currentAttack = 1;
            Context.LightAttackFirst.StartExecution(Context.Player, AttackEndedFirst);
        }

        public override void OnStaggered()
        {
            switch (_currentAttack)
            {
                case 1:
                    Context.LightAttackFirst.InterruptAttack();
                    break;
                case 2:
                    Context.LightAttackSecond.InterruptAttack();
                    break;
            }
            base.OnStaggered();
        }

        private void UnlockActions()
        {
            Context.CanDash.TryUnlock(this);
            Context.CanJump.TryUnlock(this);
            Context.CanAttack.TryUnlock(this);
        }
    }
}