using System.Collections;
using UnityEngine;

namespace Attacking.Implementation
{
    public class TestAttackExecutor : AttackExecutor
    {
        [SerializeField] private GameObject weaponThing;

        protected override IEnumerator Execute(IHitSource source)
        {
            Hitbox.EnableWith(hit =>
            {
                hit.ReceiveHit(source, new HitInfo() { Damage = 5 });
            });
            weaponThing.SetActive(true); 
            yield return new WaitForSeconds(0.2f);
            Hitbox.DisableAndClear();
            weaponThing.SetActive(false); 
        }
    }
}