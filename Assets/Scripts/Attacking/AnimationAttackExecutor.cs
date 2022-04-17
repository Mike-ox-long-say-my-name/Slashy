using System.Collections;
using UnityEngine;
using Utilities;

namespace Attacking
{
    public abstract class AnimationAttackExecutor : AttackExecutor
    {
        [SerializeField] private AttackAnimationEventDispatcher eventDispatcher;

        private readonly Trigger _attackEnded = new Trigger();

        protected override IEnumerator Execute(IHitSource source)
        {
            _attackEnded.Reset();

            eventDispatcher.SetAnimationShouldEnableHitbox(() => OnShouldEnableHitbox(source));
            eventDispatcher.SetAnimationShouldDisableHitbox(() => OnShouldDisableHitbox(source));
            eventDispatcher.SetAnimationShouldEndAttack(OnShouldEndAttack);
             
            while (!_attackEnded.CheckAndReset())
            {
                OnAttackTick(source);
                yield return null;
            }
        }

        protected abstract void OnAttackTick(IHitSource source);

        protected abstract void OnShouldEnableHitbox(IHitSource source);

        protected abstract void OnShouldDisableHitbox(IHitSource source);

        protected override void OnAttackEnded(bool interrupted)
        {
            base.OnAttackEnded(interrupted);
            EndAttack();
        }

        protected virtual void OnShouldEndAttack()
        {
            EndAttack();
        }

        protected void EndAttack()
        {
            _attackEnded.Set();
        }
    }
}