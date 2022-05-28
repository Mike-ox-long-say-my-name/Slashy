using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueRetreat : RogueBaseState
    {
        private void SetRandomSpeedMultiplier()
        {
            var multiplier = Random.Range(0.6f, 1.2f);
            Context.AutoMovement.SetSpeedMultiplier(multiplier);
        }

        private void SetRandomRetreatTarget()
        {
            const float maxOffset = 0.8f;
            var direction = new Vector3(Random.Range(0f, maxOffset),
                0, Random.Range(0f, maxOffset)).normalized;
            var distance = Random.Range(1.4f, 3.5f);
            Context.AutoMovement.MoveTo(direction * distance);
        }

        public override void EnterState()
        {
            Context.Animator.StartWalkAnimation();
            Context.AutoMovement.LockRotationOn(Context.Player.Transform);
            Context.AutoMovement.TargetReached += OnTargetReached;

            SetRandomSpeedMultiplier();
            SetRandomRetreatTarget();
            Context.AutoMovement.SetMaxMoveTime(1.5f);
        }

        private void OnTargetReached()
        {
            var value = Random.value;
            if (value < 0.2f)
            {
                SwitchState<RogueWait>();
            }
            else if (value < 0.3f)
            {
                SwitchState<RogueJumpAtPlayer>();
            }
            else
            {
                SwitchState<RoguePursue>();
            }
        }

        public override void ExitState()
        {
            Context.Animator.EndWalkAnimation();

            Context.VelocityMovement.Stop();
            Context.AutoMovement.ResetState();
            Context.AutoMovement.UnlockRotation();
        }
    }
}