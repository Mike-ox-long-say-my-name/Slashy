using Core;
using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowPursue : WeakHollowBaseState
    {
        public override void EnterState()
        {
            var player = Context.Player.Transform;
            Context.AutoMovement.LockRotationOn(player);
            Context.Animator.StartWalkAnimation();

            var offset = Context.TryToSurroundPlayer ? GetRandomOffset() : Vector3.zero;
            Context.AutoMovement.MoveToWithOffset(player, offset);
            Context.AutoMovement.SetTargetReachedEpsilon(1);
            Context.AutoMovement.TargetReached += () => SwitchState<WeakHollowRun>();
        }

        private static Vector3 GetRandomOffset()
        {
            var distance = Random.Range(1f, 3f);
            var offsetXY = Random.insideUnitCircle.normalized * distance;
            return new Vector3(offsetXY.x, 0, offsetXY.y);
        }

        public override void UpdateState()
        {
            if (Vector3.Distance(Context.PlayerPosition.WithZeroY(), Context.transform.position.WithZeroY()) > 6)
            {
                SwitchState<WeakHollowRun>();
            }
        }

        public override void ExitState()
        {
            Context.VelocityMovement.Stop();
            Context.AutoMovement.UnlockRotation();
            Context.AutoMovement.ResetState();
            Context.Animator.EndWalkAnimation();
        }
    }
}