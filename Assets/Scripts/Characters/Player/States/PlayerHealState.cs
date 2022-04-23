using System.Collections;
using Core.Characters;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerHealState : PlayerBaseGroundedState
    {
        private Coroutine _healRoutine;

        public PlayerHealState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Context.Movement.ResetXZVelocity();

            // TODO: проиграть анимацию хилирования + привязать к ней задержку
            // Context.AnimatorComponent.SetTrigger("heal");

            _healRoutine = Context.StartCoroutine(
                HealRoutine(Context.PlayerCharacter, Context.PlayerConfig.ActiveHealRate, Context.PlayerConfig.HealStaminaConsumption));
        }

        public override void InterruptState(CharacterInterruption interruption)
        {
            Context.StopCoroutine(_healRoutine);
            base.InterruptState(interruption);
        }

        public override void UpdateState()
        {
            if (Context.Input.MoveInput.sqrMagnitude > 0)
            {
                StopHealing();
            }
        }

        private void StopHealing()
        {
            if (_healRoutine != null)
            {
                Context.StopCoroutine(_healRoutine);
                _healRoutine = null;
            }
            // TODO: добавить анимацию + привязать к ней задержку
            SwitchState(Factory.Grounded());
        }

        private IEnumerator HealRoutine(PlayerCharacter player, float healRate, float staminaConsumptionRate)
        {
            while (player.Health.Value < player.Health.MaxValue)
            {
                player.Heal(healRate * Time.deltaTime);
                player.SpendStamina(staminaConsumptionRate * Time.deltaTime);
                yield return null;
            }

            StopHealing();
        }
    }
}