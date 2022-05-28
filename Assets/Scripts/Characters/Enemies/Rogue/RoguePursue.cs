using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RoguePursue : RogueBaseState
    {
        public override void EnterState()
        {
            Context.Animator.StartWalkAnimation();

            var player = Context.Player.Transform;
            Context.AutoMovement.TargetReached += OnTargetReached;
            Context.AutoMovement.MoveTo(player);
            Context.AutoMovement.LockRotationOn(player);
            Context.AutoMovement.SetTargetReachedEpsilon(1);
        }

        private void OnTargetReached()
        {
            var value = Random.value;
            if (value < 0.2f)
            {
                SwitchState<RogueJumpAway>();
            }
            else if (value < 0.8f)
            {
                SwitchState<RogueWaitBeforeThrust>();
            }
            else
            {
                SwitchState<RogueTripleSlash>();
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