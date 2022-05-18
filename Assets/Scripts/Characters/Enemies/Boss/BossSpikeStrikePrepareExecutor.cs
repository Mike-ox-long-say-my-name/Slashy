using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Player;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossSpikeStrikePrepareExecutor : MonoAnimationAttackExecutor
    {
        [SerializeField] private BossSpike spikePrefab;
        [SerializeField] private float groundOffset;

        private class CustomHandler : DefaultAttackEventHandler
        {
            public BossSpike Spike { get; set; }
            public float GroundOffset { get; set; }

            public override void HandleEnableHitbox(IAnimationAttackExecutorContext context)
            {
                var target = PlayerManager.Instance.PlayerInfo.Transform.position;
                target.y = GroundOffset;

                var spike = Instantiate(Spike, target, Quaternion.identity);
                spike.StrikeFromGround();
            }
        }

        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            executor.EventHandler = new CustomHandler()
            {
                Spike = spikePrefab,
                GroundOffset = groundOffset
            };
        }
    }
}