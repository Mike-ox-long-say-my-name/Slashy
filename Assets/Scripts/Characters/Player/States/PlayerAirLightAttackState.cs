using Core.Attacking;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerAirLightAttackState : PlayerAirboneState
    {
        public override void EnterState()
        {
            base.EnterState();
            Context.AttackedAtThisAirTime = true;

            Context.Character.SpendStamina(Context.PlayerConfig.LightAirAttackStaminaCost);
            Context.AnimatorComponent.SetTrigger("attack");
            Context.AirboneLightAttack.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(bool _)
        {
            if (Context.VelocityMovement.Movement.IsGrounded)
            {
                SwitchState<PlayerGroundedState>();
            }
            else
            {
                SwitchState<PlayerFallState>();
            }
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.AirboneLightAttack.InterruptAttack();
            base.OnStaggered(info);
        }

        public override void OnDeath(HitInfo info)
        {
            Context.AirboneLightAttack.InterruptAttack();
            base.OnDeath(info);
        }
    }
}