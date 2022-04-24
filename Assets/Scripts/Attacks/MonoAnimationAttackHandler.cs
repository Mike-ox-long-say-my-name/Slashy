using Core.Attacking;
using UnityEngine;

namespace Attacks
{
    [DisallowMultipleComponent]
    public class MonoAnimationAttackExecutor : MonoAttackHandler, IMonoAttackAnimationEventReceiver
    {
        protected override IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            return new AnimationAttackExecutor(host, attackbox);
        }

        public IAttackAnimationEventReceiver Receiver => Executor as IAttackAnimationEventReceiver;
    }
}