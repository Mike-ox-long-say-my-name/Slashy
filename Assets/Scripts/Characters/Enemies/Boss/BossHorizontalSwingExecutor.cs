using Characters.Enemies.Rogue;
using Core;
using Core.Attacking;
using Core.Attacking.Mono;
using Core.Modules;

namespace Characters.Enemies.Boss
{
    public class BossHorizontalSwingExecutor : MonoAnimationAttackExecutor
    {
        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            var lazyPlayer = Container.Get<IPlayerFactory>().GetLazyPlayer();
            var movement = GetComponentInParent<MixinVelocityMovement>().VelocityMovement;
            executor.EventHandler = new TargetLockedAttackHandler(lazyPlayer.Value.Transform, movement)
            {
                MoveOnAttack = null,
                RotateOnAttack = true
            };
        }
    }
}