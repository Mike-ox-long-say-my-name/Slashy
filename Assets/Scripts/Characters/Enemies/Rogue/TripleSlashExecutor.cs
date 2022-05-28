using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Modules;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class TripleSlashExecutor : MonoAnimationAttackExecutor
    {
        private class CustomHandler : TargetLockedAttackHandler
        {
            public CustomHandler(Transform target, IVelocityMovement movement) : base(target, movement)
            {
            }

            public override void HandleEnableHitbox(IAnimationAttackExecutorContext context)
            {
                context.Attackbox.Disable();
                base.HandleEnableHitbox(context);
            }
        }

        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            var movement = GetComponentInParent<MixinVelocityMovement>().VelocityMovement;
            var lazyPlayer = Container.Get<IPlayerFactory>().GetLazyPlayer();

            executor.EventHandler =
                new CustomHandler(lazyPlayer.Value.Transform, movement)
                {
                    MoveOnAttack = 0.4f,
                    RotateOnAttack = true
                };
        }
    }
}