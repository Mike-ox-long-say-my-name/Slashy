using Characters.Enemies.Rogue;
using Core.Attacking;
using Core.Attacking.Mono;
using Core.Modules;
using Core.Player;

namespace Characters.Enemies.Boss
{
    public class BossHorizontalSwingExecutor : MonoAnimationAttackExecutor
    {
        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            var player = PlayerManager.Instance.PlayerInfo.Transform;
            var movement = GetComponentInParent<MixinVelocityMovement>().VelocityMovement;
            executor.EventHandler = new TargetLockedAttackHandler(player, movement)
            {
                MoveOnAttack = null,
                RotateOnAttack = true
            };
        }
    }
}