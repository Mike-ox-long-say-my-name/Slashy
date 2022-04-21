using System.Collections;
using Attacking;
using Codice.CM.SEIDInfo;
using Core;
using UnityEngine;

namespace Characters.Enemies
{
    public class PunchAttack : AttackExecutor
    {
        [SerializeField] private GameObject cube;
        [SerializeField] private DamageInfo damageInfo;

        protected override IEnumerator Execute(IHitSource source)
        {
            yield return new WaitForSeconds(0.3f);
            cube.SetActive(true);
            Hitbox.EnableWith(hit => hit.ReceiveHit(new HitInfo
            {
                DamageInfo = damageInfo,
                HitSource = source
            }));
            yield return new WaitForFixedUpdate();
            Hitbox.DisableAndClear();
            yield return new WaitForSeconds(0.3f);
            cube.SetActive(false);
        }
    }
}