using Core.Attacking;
using Core.Characters.Interfaces;
using Core.Player.Interfaces;
using System.Collections;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerHealState : PlayerBaseGroundedState
    {
        private Coroutine _healRoutine;

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            Context.Animator.SetBool("is-healing", true);

            _healRoutine = Context.StartCoroutine(
                HealRoutine(
                    Context.PlayerCharacter,
                    Context.PlayerConfig.ActiveHealRate,
                    Context.PlayerConfig.HealStaminaConsumption)
                );

            Context.HealSource.Play();
        }

        public override void UpdateState()
        {
            if (Context.Input.MoveInput.sqrMagnitude > 0)
            {
                SwitchState<PlayerGroundedState>();
            }

            CheckStateSwitch();
        }

        private void CheckStateSwitch()
        {
            if (!Context.VelocityMovement.BaseMovement.IsGrounded)
            {
                SwitchState<PlayerFallState>();
            }
            else if (Context.ShouldDash)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerDashState>();
            }
            else if (Context.ShouldJump)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerJumpState>();
            }
            else if (Context.ShouldLightAttack)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerGroundLightAttackState>();
            }
            else if (Context.ShouldStrongAttack)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerGroundStrongAttackState>();
            }
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-healing", false);
            StopHealing();
            
            Context.HealSource.Stop();
        }

        private void StopHealing()
        {
            if (_healRoutine == null)
            {
                return;
            }

            Context.StopCoroutine(_healRoutine);
            _healRoutine = null;
        }

        private IEnumerator HealRoutine(IPlayerCharacter player, float healRate, float staminaConsumptionRate)
        {
            while (player.Character.Health.Value < player.Character.Health.MaxValue && player.HasStamina())
            {
                player.Character.Health.Recover(healRate * Time.deltaTime);
                player.Stamina.Spend(staminaConsumptionRate * Time.deltaTime);
                yield return null;
            }

            SwitchState<PlayerGroundedState>();
        }
    }
}