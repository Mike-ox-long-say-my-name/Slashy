using Core.Characters.Interfaces;
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
            Context.Animator.StartHealAnimation();
            
            PreventStaminaRecoveryBeforeFirstTick();
            
            _healRoutine = Context.StartCoroutine(
                HealRoutine(
                    Context.Character.Health,
                    Context.PlayerConfig.HealPerTick,
                    Context.PlayerConfig.HealTickInterval)
            );

            Context.HealAudioSource.Play();
        }

        private void PreventStaminaRecoveryBeforeFirstTick()
        {
            Context.Stamina.Spend(0.1f);
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
            StopHealing();
            Context.Animator.EndHealAnimation();
            Context.HealAudioSource.Stop();
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

        private IEnumerator HealRoutine(IResource health, float healPerTick, float tickInterval)
        {
            while (health.Value < health.MaxValue &&
                   Context.ResourceSpender.HasEnoughResourcesFor(PlayerResourceAction.HealTick))
            {
                yield return new WaitForSeconds(tickInterval);

                health.Recover(healPerTick);
                Context.Purity.Spend(Context.PlayerConfig.HealTickPurityCost);
                Context.ResourceSpender.SpendFor(PlayerResourceAction.HealTick);
            }

            SwitchState<PlayerGroundedState>();
        }
    }
}