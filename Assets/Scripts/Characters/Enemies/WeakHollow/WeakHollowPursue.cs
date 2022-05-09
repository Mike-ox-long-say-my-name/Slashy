using Core.Characters;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowPursue : WeakHollowBaseState
    {
        public override void EnterState()
        { 
            Context.AnimatorComponent.SetBool("is-walking", true);

            var offset = GetRandomOffset();
            Context.AutoMovement.MoveToWithOffset(Context.PlayerInfo.Transform, offset);
            Context.AutoMovement.SetTargetReachedEpsilon(3);
            Context.AutoMovement.TargetReached += SwitchState<WeakHollowCharge>;
        }

        private Vector3 GetRandomOffset()
        {
            var direction = Context.transform.position - Context.PlayerPosition;
            var normal = new Vector3(Mathf.Sign(-direction.z), 0, Mathf.Sign(direction.x)).normalized;
            const float distance = 2f;
            var sign = (Random.value < 0.5) ? -1 : 1;
            return distance * sign * normal;
        }

        public override void ExitState()
        {
            Context.AutoMovement.ResetState();
            Context.AnimatorComponent.SetBool("is-walking", false);
        }
    }
}