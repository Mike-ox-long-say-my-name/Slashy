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

            // TODO: проиграть анимацию хилирования
            // Context.AnimatorComponent.SetTrigger("heal");
            _healRoutine = Context.StartCoroutine(
                HealRoutine(Context.Player, Context.ActionConfig.ActiveHealRate, Context.ActionConfig.HealStaminaConsumption));
        }

        public override void OnStaggered()
        {
            Context.StopCoroutine(_healRoutine);
            base.OnStaggered();
        }

        private IEnumerator HealRoutine(PlayerCharacter player, float healRate, float staminaConsumptionRate)
        {
            while (player.Health.Value < player.Health.MaxValue)
            {
                player.Heal(healRate * Time.deltaTime);
                player.SpendStamina(staminaConsumptionRate * Time.deltaTime);
                yield return null;
            }
            
            Context.CanRotate.TryUnlock(this);
            SwitchState(Factory.Grounded());
        }
    }
}