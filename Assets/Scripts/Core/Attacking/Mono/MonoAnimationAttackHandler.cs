using System;
using Core.Attacking.Interfaces;
using Core.DependencyInjection;
using UnityEngine;  

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    public class MonoAnimationAttackExecutor : MonoAttackHandler
    {
        protected override IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            return new AnimationAttackExecutor(host, attackbox);
        }

        public IAttackAnimationEventReceiver Receiver => Executor as IAttackAnimationEventReceiver;
    }
}