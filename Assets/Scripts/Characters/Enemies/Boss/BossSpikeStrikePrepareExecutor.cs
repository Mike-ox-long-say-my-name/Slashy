using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossSpikeStrikePrepareExecutor : MonoAnimationAttackExecutor
    {
        [SerializeField] private BossSpike spikePrefab;
        [SerializeField] private float groundOffset;

        private class CustomHandler : DefaultAttackEventHandler
        {
            private readonly Transform _player;
            public BossSpike Spike { get; set; }
            public float GroundOffset { get; set; }
            private Vector3 _target;

            public CustomHandler(Transform player)
            {
                _player = player;
            }
            
            public void AimAtPlayer()
            {
                var target = _player.position;
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
            var lazyPlayer = Container.Get<IPlayerFactory>().GetLazyPlayer();
            var handler = new CustomHandler(lazyPlayer.Value.Transform)
            {
                Spike = spikePrefab,
                GroundOffset = groundOffset
            };
            executor.EventHandler = handler;

            var animationEvents = GetComponentInParent<BossAnimationEvents>();
            animationEvents.AimSpike.AddListener(handler.AimAtPlayer);
        }
    }
}