using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    public class MonoAnimationAttackExecutor : MonoAttackHandler
    {
        protected virtual AnimationAttackExecutor CreateAnimationAttackExecutor(ICoroutineHost host,
            IAttackbox attackbox)
        {
            return new AnimationAttackExecutor(host, attackbox);
        }

        public IAttackAnimationEventReceiver Receiver => Executor as IAttackAnimationEventReceiver;

        protected override IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            return CreateAnimationAttackExecutor(host, attackbox);
        }
    }
}