using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowPursue : WeakHollowBaseState
    {
        public override void EnterState()
        {
            var player = Context.PlayerInfo.Transform;
            Context.AutoMovement.LockRotationOn(player);
            Context.AnimatorComponent.SetBool("is-walking", true);

            var offset = Context.TryToSurroundPlayer ? GetRandomOffset() : Vector3.zero;
            Context.AutoMovement.MoveToWithOffset(player, offset);
            Context.AutoMovement.SetTargetReachedEpsilon(1);
            // TODO: keep distance
            Context.AutoMovement.TargetReached += SwitchState<WeakHollowCharge>;
        }

        private static Vector3 GetRandomOffset()
        {
            var distance = Random.Range(2f, 6f);
            return Random.insideUnitCircle.normalized * distance;
        }

        public override void ExitState()
        {
            Context.VelocityMovement.Stop();
            Context.AutoMovement.UnlockRotation();
            Context.AutoMovement.ResetState();
            Context.AnimatorComponent.SetBool("is-walking", false);
        }
    }
}