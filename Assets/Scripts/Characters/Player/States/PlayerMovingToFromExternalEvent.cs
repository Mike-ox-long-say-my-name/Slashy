using Core;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerMovingToFromExternalEvent : PlayerBaseGroundedState
    {
        private Vector3 _targetPosition;

        private bool IsMovingToBonfire => Context.BonfireToTouch != null;

        public override void EnterState()
        {
            Context.Hurtbox.Disable();
            Context.Animator.StartWalkAnimation();

            if (IsMovingToBonfire)
            {
                var bonfire = Context.BonfireToTouch;
                _targetPosition = bonfire.GetPlayerAnimationPosition();
            }
            else
            {
                _targetPosition = Context.WarpPosition!.Value;
            }
        }

        public override void UpdateState()
        {
            var player = Context.Position.WithZeroY();

            if (Vector3.Distance(player, _targetPosition) <= 0.1f)
            {
                OnTargetReached();
                return;
            }

            var direction = (_targetPosition - player).normalized;
            Context.VelocityMovement.Move(direction);
        }

        private void OnTargetReached()
        {
            if (IsMovingToBonfire)
            {
                SwitchState<PlayerTouchingBonfireState>();
            }
            else
            {
                // 1. Уровень автоматически сменится
                // 2. Уровень уже поменялся и все хорошо
                SwitchState<PlayerGroundedState>();
            }
        }

        public override void ExitState()
        {
            Context.VelocityMovement.Stop();
            Context.Animator.EndWalkAnimation();
        }
    }
}