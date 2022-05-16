using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class TargetLockedAttackHandler : DefaultAttackEventHandler
    {
        private readonly Transform _target;
        private readonly IVelocityMovement _movement;

        public bool RotateOnAttack { get; set; } = true;
        public float? MoveOnAttack { get; set; } = null;

        public TargetLockedAttackHandler(Transform target, IVelocityMovement movement)
        {
            _target = target;
            _movement = movement;
        }

        private void RotateTowardsTarget()
        {
            var baseMovement = _movement.BaseMovement;
            var direction = _target.position.x - baseMovement.Transform.position.x;
            baseMovement.Rotate(direction);
        }

        private void MoveTowardsTarget(float distance)
        {
            var baseMovement = _movement.BaseMovement;
            var direction = _target.position.x - baseMovement.Transform.position.x;
            var move = new Vector3(Mathf.Sign(direction) * distance, 0, 0);
            baseMovement.Move(move);
        }

        public override void HandleEnableHitbox(IAnimationAttackExecutorContext context)
        {
            if (RotateOnAttack)
            {
                RotateTowardsTarget();
            }

            if (MoveOnAttack != null)
            {
                MoveTowardsTarget(MoveOnAttack.Value);
            }

            base.HandleEnableHitbox(context);
        }
    }
}