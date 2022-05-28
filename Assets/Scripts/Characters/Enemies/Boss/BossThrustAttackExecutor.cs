using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Modules;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossThrustAttackExecutor : MonoAnimationAttackExecutor
    {
        private class CustomHandler : DefaultAttackEventHandler
        {
            private readonly Transform _target;
            private readonly IVelocityMovement _movement;

            public float? MoveOnAttack { get; set; }

            public CustomHandler(Transform target, IVelocityMovement movement)
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

            private void MoveTowardsTarget(float maxDistance)
            {
                var baseMovement = _movement.BaseMovement;

                var self = baseMovement.Transform.position.WithZeroY();
                var target = _target.position.WithZeroY();

                var direction = (target - self).normalized;
                var distance = Vector3.Distance(target, self);

                baseMovement.Move(direction * Mathf.Min(maxDistance, distance));
            }

            public override void HandleEnableHitbox(IAnimationAttackExecutorContext context)
            {
                RotateTowardsTarget();

                if (MoveOnAttack != null)
                {
                    MoveTowardsTarget(MoveOnAttack.Value);
                }

                base.HandleEnableHitbox(context);
            }
        }

        private CustomHandler _handler;

        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            var movement = GetComponentInParent<MixinVelocityMovement>().VelocityMovement;
            var lazyPlayer = Container.Get<IPlayerFactory>().GetLazyPlayer();
            executor.EventHandler = _handler = new CustomHandler(lazyPlayer.Value.Transform, movement);
        }

        public void SetMaxDashDistance(float distance)
        {
            _handler.MoveOnAttack = distance > 0 ? distance : (float?)null;
        }
    }
}