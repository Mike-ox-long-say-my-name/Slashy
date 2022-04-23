using System.Collections;
using Attacking;
using Core;
using UnityEngine;

namespace Characters.Enemies
{
    public class TestAttackExecutor : AttackExecutor
    {
        [SerializeField] private GameObject weaponThing;

        protected override IEnumerator Execute(IHitSource source)
        {
            Hitbox.EnableWith(hit =>
            {
                hit.ReceiveHit(new HitInfo()
                {
                    HitSource = source,
                    DamageInfo = new DamageInfo
                    {
                        damage = 10,
                        balanceDamage = 5,
                        pushStrength = 1,
                        staggerTime = 1
                    }
                });
            });
            weaponThing.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            Hitbox.DisableAndClear();
            weaponThing.SetActive(false);
        }
    }
}