using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossJumpAttackExecutor : MonoAnimationAttackExecutor
    {
        [SerializeField] private GameObject spikes;

        private class CustomHandler : DefaultAttackEventHandler
        {
            public GameObject Spikes { get; set; }
            public Transform Parent { get; set; }

            private GameObject _createdSpikes;

            public override void HandleEnableHitbox(IAnimationAttackExecutorContext context)
            {
                base.HandleEnableHitbox(context);
                _createdSpikes = Instantiate(Spikes, Parent.position, Quaternion.identity);
                _createdSpikes.hideFlags = HideFlags.HideAndDontSave;
            }

            public override void HandleAnimationEnd(IAnimationAttackExecutorContext context)
            {
                base.HandleAnimationEnd(context);
                Destroy(_createdSpikes);
            }
        }

        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            executor.EventHandler = new CustomHandler
            {
                Spikes = spikes,
                Parent = transform
            };
        }
    }
}