using Core;
using Core.Attacking;
using Core.Attacking.Mono;
using Core.Modules;

namespace Characters.Enemies.Rogue
{
    public class ThrustAttackExecutor : MonoAnimationAttackExecutor
    {
        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            var movement = GetComponentInParent<MixinVelocityMovement>().VelocityMovement;
            var lazyPlayer = Container.Get<IPlayerFactory>().GetLazyPlayer();

            var handler =
                new TargetLockedAttackHandler(lazyPlayer.Value.Transform, movement)
                {
                    MoveOnAttack = 1,
                    RotateOnAttack = true
                };

            executor.EventHandler = handler;
        }
    }
}