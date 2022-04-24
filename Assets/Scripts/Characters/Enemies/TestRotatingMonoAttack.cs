using Attacks;
using Core.Attacking;
using Core.Utilities;
using System.Collections;
using UnityEngine;

namespace Characters.Enemies
{
    public class TestRotatingMonoAttack : MonoAttackHandler
    {
        private class RotatingAttackExecutor : AttackExecutor
        {
            private readonly Transform _transform;

            public RotatingAttackExecutor(Transform transform, ICoroutineHost host, IAttackbox attackbox) : base(host, attackbox)
            {
                _transform = transform;
            }

            private readonly TimedTrigger _attackClearTrigger = new TimedTrigger();

            protected override IEnumerator Execute()
            {
                Attackbox.Enable();

                while (true)
                {
                    _attackClearTrigger.Step(Time.deltaTime);
                    if (_attackClearTrigger.CheckAndReset())
                    {
                        Attackbox.ClearHits();
                    }

                    Attackbox.Transform.RotateAround(_transform.position, Vector3.up, Time.deltaTime * 125);
                    yield return new WaitForEndOfFrame();
                }
            }

            public override bool InterceptHit(IHurtbox hit)
            {
                _attackClearTrigger.SetIn(1);
                return base.InterceptHit(hit);
            }
        }

        protected override IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            return new RotatingAttackExecutor(transform, host, attackbox);
        }
    }
}
