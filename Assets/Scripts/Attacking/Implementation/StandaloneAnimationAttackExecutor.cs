using UnityEngine;

namespace Attacking.Implementation
{
    public class StandaloneAnimationAttackExecutor : AnimationAttackExecutor
    {
        [SerializeField] private HitInfo hitInfo;

        protected override void OnShouldEnableHitbox(IHitSource source)
        {
            Hitbox.EnableWith(hit => hit.ReceiveHit(source, hitInfo));
        }

        protected override void OnShouldDisableHitbox(IHitSource source)
        {
            Hitbox.DisableAndClear();
        }
    }
}