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
            AdjustToAnimation();
            Context.BonfireToTouch = null;

            Context.Animator.StartBonfireAnimation();
            Context.OnTouchedBonfire();

            _touchTimer = Timer.Start(AnimationTime, () => SwitchState<PlayerGroundedState>());
        }

        private void AdjustToAnimation()
        {
            var bonfire = Context.BonfireToTouch;
            var bonfirePositionX = bonfire.transform.position.x;
            var animationPosition = bonfire.GetPlayerAnimationPosition();

            Context.BaseMovement.SetPosition(animationPosition);
            Context.BaseMovement.Rotate(bonfirePositionX - animationPosition.x);
        }

        public override void UpdateState()
        {
            var healthRegenAmount = Context.Character.Health.MaxValue / AnimationTime * 2;
            var staminaRegenAmount = Context.Stamina.MaxValue / AnimationTime * 2;
            var purityRegenAmount = Context.Purity.MaxValue / AnimationTime * 2;

            var deltaTime = Time.deltaTime;
            
            Context.Character.Health.Recover(healthRegenAmount * deltaTime);
            Context.Stamina.Recover(staminaRegenAmount * deltaTime);
            Context.Purity.Recover(purityRegenAmount * deltaTime);

            _touchTimer.Tick(deltaTime);
        }

        public override void ExitState()
        {
            Context.Hurtbox.Enable();
            Context.Animator.EndBonfireAnimation();
        }
    }
}