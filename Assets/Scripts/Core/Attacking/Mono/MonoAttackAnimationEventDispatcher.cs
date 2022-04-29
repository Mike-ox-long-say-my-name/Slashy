using System.Collections.Generic;
using System.Linq;
using Core.Attacking.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    public class MonoAttackAnimationEventDispatcher : MonoBehaviour, IMonoAttackAnimationEventDispatcher
    {
        private List<IAttackAnimationEventReceiver> _receivers;

        private void Awake()
        {
            _receivers = GetComponentsInChildren<MonoAnimationAttackExecutor>()
                .Select(receiver => receiver.Receiver).ToList();
        }
        
        [UsedImplicitly]
        public void OnAnimationShouldEnableHitbox()
        {
            foreach (var receiver in _receivers)
            {
                receiver.ReceiveEnableHitbox();
            }
        }
        
        [UsedImplicitly]
        public void OnAnimationShouldDisableHitbox()
        {
            foreach (var receiver in _receivers)
            {
                receiver.ReceiveDisableHitbox();
            }
        }
        
        [UsedImplicitly]
        public void OnAnimationShouldEndAttack()
        {
            foreach (var receiver in _receivers)
            {
                receiver.ReceiveAnimationEnded();
            }
        }

        public void Register(IAttackAnimationEventReceiver receiver)
        {
            _receivers.Add(receiver);
        }

        public void Unregister(IAttackAnimationEventReceiver receiver)
        {
            _receivers.Remove(receiver);
        }
    }
}