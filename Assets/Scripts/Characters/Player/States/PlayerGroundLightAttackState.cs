﻿using System.Collections;
using Core.Attacking;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerGroundLightAttackState : PlayerBaseGroundedState
    {
        private bool _shouldContinueAttack;

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();

            Context.AnimatorComponent.SetTrigger("attack");
            Context.Player.Stamina.Spend(Context.PlayerConfig.LightAttackFirstStaminaCost);
            
            _shouldContinueAttack = false;

            Context.FirstLightAttack.StartAttack(AttackEndedFirst);
        }

        private void AttackEndedFirst(AttackResult result)
        {
            if (result.WasCompleted && _shouldContinueAttack)
            {
                Context.Player.Stamina.Spend(Context.PlayerConfig.LightAttackSecondStaminaCost);
                Context.SecondLightAttack.StartAttack(AttackEndedSecond);
            }
            else
            {
                AttackEndedSecond(result);
            }
        }

        public override void UpdateState()
        {
            if (!_shouldContinueAttack && Context.Player.HasStamina() && Context.Input.IsLightAttackPressed)
            {
                Context.AnimatorComponent.SetTrigger("attack");
                _shouldContinueAttack = true;
            }
        }

        private void AttackEndedSecond(AttackResult result)
        {
            if (result.WasCompleted)
            {
                Context.StartCoroutine(RecoveryRoutine(Context.PlayerConfig.LightAttackRecovery));
                SwitchState<PlayerGroundedState>();
            }
            else
            {
                Context.AnimatorComponent.ResetTrigger("attack");
            }
        }

        private IEnumerator RecoveryRoutine(float recoverTime)
        {
            Context.LightAttackRecoveryLock.Lock(this);
            yield return new WaitForSeconds(recoverTime);
            Context.LightAttackRecoveryLock.TryUnlock(this);
        }
    }
}