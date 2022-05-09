using System.Collections;
using Core.Attacking;
using Core.Characters.Interfaces;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerHealState : PlayerBaseGroundedState
    {
        private Coroutine _healRoutine;

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            
            Context.AnimatorComponent.SetBool("is-healing", true);

            _healRoutine = Context.StartCoroutine(
                HealRoutine(
                    Context.Player,
                    Context.PlayerConfig.ActiveHealRate,
                    Context.PlayerConfig.HealStaminaConsumption)
                );
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.StopCoroutine(_healRoutine);
            base.OnStaggered(info);
        }

        public override void OnDeath(HitInfo info)
        {
            Context.StopCoroutine(_healRoutine);
            base.OnDeath(info);
        }

        public override void UpdateState()
        {
            if (Context.Input.MoveInput.sqrMagnitude > 0)
            {
                StopHealing();
            }
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-healing", false);
        }

        private void StopHealing()
        {
            if (_healRoutine != null)
            {
                Context.StopCoroutine(_healRoutine);
                _healRoutine = null;
            }

            SwitchState<PlayerGroundedState>();
        }

        private IEnumerator HealRoutine(IPlayerCharacter player, float healRate, float staminaConsumptionRate)
        {
            while (player.Character.Health.Value < player.Character.Health.MaxValue)
            {
                player.Character.Health.Recover(healRate * Time.deltaTime);
                player.Stamina.Spend(staminaConsumptionRate * Time.deltaTime);
                yield return null;
            }

            StopHealing();
        }
    }
}