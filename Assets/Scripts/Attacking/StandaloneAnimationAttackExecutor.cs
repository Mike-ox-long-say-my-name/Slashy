using Core;
using UnityEngine;

namespace Attacking
{
    public class StandaloneAnimationAttackExecutor : AnimationAttackExecutor
    {
        [SerializeField] private DamageInfo damageInfo;

        protected override void OnAttackTick(IHitSource source)
        {
        }

        protected override void OnShouldEnableHitbox(IHitSource source)
        {
            Hitbox.EnableWith(hit =>
                hit.ReceiveHit(new HitInfo { DamageInfo = damageInfo, HitSource = source }));
        }

        protected override void OnShouldDisableHitbox(IHitSource source)
        {
            Hitbox.DisableAndClear();
        }
    }
}