using Core.Attacking;
using Core.Attacking.Mono;
using Core.Modules;
using Core.Player;

namespace Characters.Enemies.Rogue
{
    public class ThrustAttackExecutor : MonoAnimationAttackExecutor
    {
        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            var movement = GetComponentInParent<MixinVelocityMovement>().VelocityMovement;
            var handler =
                new TargetLockedAttackHandler(PlayerManager.Instance.PlayerInfo.Transform, movement)
                {
                    MoveOnAttack = 1,
                    RotateOnAttack = true
                };

            executor.EventHandler = handler;
        }
    }
}