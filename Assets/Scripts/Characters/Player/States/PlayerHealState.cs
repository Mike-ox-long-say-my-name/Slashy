using System.Collections;
using Core;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerHealState : PlayerBaseState
    {
        private Coroutine _healRoutine;

        public PlayerHealState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            Context.CanRotate.Lock(this);
            Context.Movement.ResetXZVelocity();

            // TODO: проиграть анимацию хилирования + привязать к ней задержку
            // Context.AnimatorComponent.SetTrigger("heal");
            _healRoutine = Context.StartCoroutine(
                HealRoutine(Context.Player, Context.ActionConfig.ActiveHealRate, Context.ActionConfig.HealStaminaConsumption));
        }

        public override void OnStaggered()
        {
            StopHealing();
            base.OnStaggered();
        }

        public override void UpdateState()
        {
            if (!Context.IsStaggered && Context.MoveInput.sqrMagnitude > 0)
            {
                StopHealing();
                SwitchState(Factory.Grounded());
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
            Context.CanRotate.TryUnlock(this);
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
            SwitchState(Factory.Grounded());
        }
    }
}