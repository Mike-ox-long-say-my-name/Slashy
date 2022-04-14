using System.Collections;
using UnityEngine;
using Utilities;

namespace Attacking.Implementation
{
    public class TestRotatingAttack : AttackExecutor
    {
        private readonly TimedTrigger _attackClearTrigger = new TimedTrigger();

        protected override IEnumerator Execute(IHitSource source)
        {
            Hitbox.EnableWith(hit =>
            {
                hit.ReceiveHit(source, new HitInfo() { damage = 10 });
                _attackClearTrigger.SetIn(1);
            });
        
            while (true)
            {
                _attackClearTrigger.Step(Time.deltaTime);
                if (_attackClearTrigger.CheckAndReset())
                {
                    Hitbox.ClearHits();
                }
            
                Hitbox.transform.RotateAround(source.Transform.position, Vector3.up, Time.deltaTime * 100);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
