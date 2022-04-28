using System;
using Core.Attacking.Interfaces;
using Core.DependencyInjection;
using UnityEngine;  

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    public class MonoAnimationAttackExecutor : MonoAttackHandler, IMonoAttackAnimationEventReceiver
    {
        protected override IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            return new AnimationAttackExecutor(host, attackbox);
        }

        IAttackAnimationEventReceiver IMonoWrapper<IAttackAnimationEventReceiver>.Resolve()
        {
            return Resolve() as IAttackAnimationEventReceiver;
        }
    }
}