using System.Collections;
using Attacking;
using Core;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies
{
    public class TestRotatingAttack : AttackExecutor
    {
        private readonly TimedTrigger _attackClearTrigger = new TimedTrigger();

        protected override IEnumerator Execute(IHitSource source)
        {
            Hitbox.EnableWith(hit =>
            {
                hit.ReceiveHit(new HitInfo()
                {
                    HitSource = source,
                    DamageInfo = new DamageInfo
                    {
                        damage = 15,
                        balanceDamage = 10,
                        pushStrength = 2,
                        staggerTime = 1
                    }
                });
                _attackClearTrigger.SetIn(1);
            });

            while (true)
            {
                _attackClearTrigger.Step(Time.deltaTime);
                if (_attackClearTrigger.CheckAndReset())
                {
                    Hitbox.ClearHits();
                }

                Hitbox.transform.RotateAround(source.Transform.position, Vector3.up, Time.deltaTime * 125);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
