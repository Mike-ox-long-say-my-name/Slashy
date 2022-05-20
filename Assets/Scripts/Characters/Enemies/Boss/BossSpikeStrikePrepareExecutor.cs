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
            private Vector3 _target;

            public void AimAtPlayer()
            {
                var target = PlayerManager.Instance.PlayerInfo.Transform.position;
                target.y = GroundOffset;
                _target = target;
            }

            public override void HandleEnableHitbox(IAnimationAttackExecutorContext context)
            {
                var spike = Instantiate(Spike, _target, Quaternion.identity);
                spike.StrikeFromGround();
            }
        }

        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            var handler = new CustomHandler
            {
                Spike = spikePrefab,
                GroundOffset = groundOffset
            };
            executor.EventHandler = handler;

            var animationEvents = GetComponentInParent<BossAnimationEventDispatcher>();
            animationEvents.AimSpike.AddListener(handler.AimAtPlayer);
        }
    }
}