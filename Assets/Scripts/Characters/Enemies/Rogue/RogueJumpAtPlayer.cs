using Core;
using Core.Attacking;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueJumpAtPlayer : RogueBaseState
    {
        private bool _updatePassed;
        private Vector3 _direction;
        private const float Multiplier = 1.5f;

        public override void EnterState()
        {
            Context.Animator.StartJumpAtAnimation();

            _updatePassed = false;

            Context.AutoMovement.UnlockRotation();
            Context.VelocityMovement.AutoResetVelocity = false;

            var targetPosition = Context.PlayerPosition;
            if (Context.TryPredictPlayerMovement)
            {
                var predictedMovement = GetPredictedPlayerMovement(Multiplier);
                targetPosition += predictedMovement;
            }

            _direction = (targetPosition.WithZeroY() - Context.transform.position.WithZeroY()).normalized;

            PrewarmVelocityMovement();
            Context.JumpHandler.Jump();
        }

        private void PrewarmVelocityMovement()
        {
            Context.VelocityMovement.Move(_direction * Multiplier);
        }

        private Vector3 GetPredictedPlayerMovement(float multiplier)
        {
            var distance = Vector3.Distance(Context.PlayerPosition.WithZeroY(),
                Context.transform.position.WithZeroY());
            return Context.Player.VelocityMovement.Velocity.WithZeroY().normalized *
                   Mathf.Sqrt(distance * multiplier);
        }

        public override void UpdateState()
        {
            if (_updatePassed && Context.VelocityMovement.BaseMovement.IsGrounded)
            {
                SwitchState<RogueWait>();
            }
            else if (Vector3.Distance(Context.PlayerPosition.WithZeroY(), Context.transform.position.WithZeroY()) < 1)
            {
                Context.VelocityMovement.ResetGravity();
                SwitchState<RogueJumpAttack>();
            }
            else
            {
                Context.VelocityMovement.Move(_direction * Multiplier);
            }

            _updatePassed = true;
        }

        public override void OnStaggered(HitInfo info)
        {
            SwitchState<RogueAirStagger>();
        }

        public override void ExitState()
        {
            Context.Animator.EndJumpAtAnimation();
            Context.VelocityMovement.Stop();

            Context.VelocityMovement.AutoResetVelocity = true;
        }
    }
}