using Core;
using UnityEngine;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowPursue : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.Animator.StartWalkAnimation();
        }

        public override void ExitState()
        {
            Context.Animator.EndWalkAnimation();
        }

        public override void UpdateState()
        {
            var player = Context.PlayerPosition.WithZeroY();
            var self = Context.transform.position.WithZeroY();
            var direction = player - self;
            
            if (direction.magnitude > 1.2f)
            {
                direction.Normalize();
                Context.VelocityMovement.Move(new Vector3(direction.x, 0, direction.z));
            }
            else
            {
                Context.VelocityMovement.BaseMovement.Rotate(direction.x);
                SwitchState<ExplodingHollowAttack>();
            }
        }
    }
}