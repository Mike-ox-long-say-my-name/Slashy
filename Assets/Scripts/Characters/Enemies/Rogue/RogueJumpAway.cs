using Core.Attacking;
using Core.Characters;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueJumpAway : RogueBaseState
    {
        private bool _updatePassed;

        public override void EnterState()
        {
            Context.Animator.StartJumpAwayAnimation();

            var jumpLocation = GetRandomJumpLocation();
            PrewarmVelocityMovement(jumpLocation);
            
            Context.JumpHandler.Jump();
            _updatePassed = false;

            Context.VelocityMovement.AutoResetVelocity = false;

            Context.AutoMovement.TargetReached += OnTargetReached;
            Context.AutoMovement.MoveTo(jumpLocation);
            Context.AutoMovement.LockRotationOn(Context.Player.Transform);
            Context.AutoMovement.SetMaxMoveTime(1);
            Context.AutoMovement.SetSpeedMultiplier(1);
        }

        private void PrewarmVelocityMovement(Vector3 jumpLocation)
        {
            Context.VelocityMovement.Move((jumpLocation - Context.transform.position).normalized);
        }

        public override void UpdateState()
        {
            if (_updatePassed && Context.BaseMovement.IsGrounded)
            {
                OnTargetReached();
            }

            _updatePassed = true;
        }

        private void OnTargetReached()
        {
            if (Random.value < 0.7f)
            {
                SwitchState<RoguePursue>();
            }
            else
            {
                SwitchState<RogueWait>();
            }
        }

        public override void OnStaggered(HitInfo info)
        {
            SwitchState<RogueAirStagger>();
        }

        private Vector3 GetRandomJumpLocation()
        {
            var distance = Random.Range(Context.MinJumpAwayDistance, Context.MaxJumpAwayDistance);
            var sign = Mathf.Sign(Context.PlayerPosition.x - Context.transform.position.x);
            var zOffset = Random.Range(0f, Context.MaxJumpAwayZRatio);
            var location = new Vector3(sign, 0, zOffset).normalized;
            return location * distance;
        }

        public override void ExitState()
        {
            Context.Animator.EndJumpAwayAnimation();
            Context.AutoMovement.ResetState();

            Context.VelocityMovement.AutoResetVelocity = true;
        }
    }
}