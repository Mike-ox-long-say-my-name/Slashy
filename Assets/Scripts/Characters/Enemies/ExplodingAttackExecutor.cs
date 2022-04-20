using System.Collections;
using Attacking;
using Core;
using UnityEngine;

namespace Characters.Enemies
{
    public class ExplodingAttackExecutor : AttackExecutor
    {
        [SerializeField] private DamageInfo damageInfo;

        protected override IEnumerator Execute(IHitSource source)
        {
            yield return new WaitForSeconds(1.5f);
            Hitbox.EnableWith(hit => hit.ReceiveHit(new HitInfo()
            {
                DamageInfo = damageInfo,
                HitSource = source
            }));
            yield return new WaitForSeconds(0.1f);
            Hitbox.DisableAndClear();
        }
    }
}