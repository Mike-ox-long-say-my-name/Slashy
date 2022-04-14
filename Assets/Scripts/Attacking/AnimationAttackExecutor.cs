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
            eventDispatcher.AnimationShouldEnableHitbox.AddListener(() => OnShouldEnableHitbox(source));
            eventDispatcher.AnimationShouldDisableHitbox.AddListener(() => OnShouldDisableHitbox(source));
            eventDispatcher.AnimationShouldEndAttack.AddListener(OnShouldEndAttack);

            while (!_attackEnded.CheckAndReset())
            {
                yield return null;
            }
        }

        protected abstract void OnShouldEnableHitbox(IHitSource source);

        protected abstract void OnShouldDisableHitbox(IHitSource source);
        
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