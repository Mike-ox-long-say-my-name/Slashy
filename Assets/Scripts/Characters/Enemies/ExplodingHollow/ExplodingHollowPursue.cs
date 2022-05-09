using UnityEngine;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowPursue : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetBool("is-walking", true);
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-walking", false);
        }

        public override void UpdateState()
        {
            var player = Context.PlayerPosition;
            var self = Context.transform.position;
            var direction = player - self;
            direction.y = 0;
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