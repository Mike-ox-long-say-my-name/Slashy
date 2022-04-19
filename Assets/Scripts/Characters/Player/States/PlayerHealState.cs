using System.Collections;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerHealState : PlayerBaseState
    {
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
            Context.StartCoroutine(
                HealRoutine(Context.Player, Context.ActionConfig.ActiveHealRate, Context.ActionConfig.HealStaminaConsumption));
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