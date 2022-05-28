using System.Collections;
using Core.Attacking;
using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerGroundLightAttackState : PlayerBaseGroundedState
    {
        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            Context.Animator.PlayFirstGroundLightAttack();
            Context.ResourceSpender.SpendFor(PlayerResourceAction.FirstLightAttack);
            Context.FirstLightAttack.StartAttack(OnFirstAttackEnded);
        }

        private void OnFirstAttackEnded(AttackResult result)
        {
            if (ShouldEndCombo(result))
            {
                OnSecondAttackEnded(result);
                return;
            }

            Context.Animator.PlaySecondGroundLightAttack();
            Context.ResourceSpender.SpendFor(PlayerResourceAction.SecondLightAttack);
            Context.SecondLightAttack.StartAttack(OnSecondAttackEnded);
        }

        private bool ShouldEndCombo(AttackResult result)
        {
            return !result.WasCompleted || !Context.Stamina.HasAny() || !Context.Input.IsLightAttackPressed;
        }

        private void OnSecondAttackEnded(AttackResult result)
        {
            OnWholeAttackEnded(result);
        }

        private void OnWholeAttackEnded(AttackResult result)
        {
            if (result.WasCompleted)
            {
                SwitchState<PlayerGroundedState>();
            }
        }
    }
}