using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerTouchingBonfireState : PlayerBaseState
    {
        private Timer _touchTimer;
        private const float AnimationTime = 1.5f;

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            Context.Animator.SetBool("is-saving", true);

            _touchTimer = Timer.Start(AnimationTime, SwitchState<PlayerGroundedState>);
        }

        public override void UpdateState()
        {
            var healthRegenAmount = Context.Player.Character.Health.MaxValue / (AnimationTime * 1.3f);
            var staminaRegenAmount = Context.Player.Stamina.MaxValue / (AnimationTime * 1.3f);

            var deltaTime = Time.deltaTime;
            Context.Player.Character.Health.Recover(healthRegenAmount * deltaTime);
            Context.Player.Stamina.Recover(staminaRegenAmount * deltaTime);
            _touchTimer.Tick(deltaTime);
        }

        public override void ExitState()
        {
            Context.Hurtbox.Enable();
            Context.Animator.SetBool("is-saving", false);
        }
    }
}