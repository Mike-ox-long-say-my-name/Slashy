using System.Collections.Generic;
using System.Linq;
using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    public class MonoAttackAnimationEventDispatcher : MonoBehaviour, IMonoAttackAnimationEventDispatcher
    {
        private List<IAttackAnimationEventReceiver> _receivers;

        private void Awake()
        {
            _receivers = GetComponentsInChildren<IMonoAttackAnimationEventReceiver>()
                .Select(receiver => receiver.Receiver).ToList();
        }

        public void OnAnimationShouldEnableHitbox()
        {
            foreach (var receiver in _receivers)
            {
                receiver.ReceiveEnableHitbox();
            }
        }

        public void OnAnimationShouldDisableHitbox()
        {
            foreach (var receiver in _receivers)
            {
                receiver.ReceiveDisableHitbox();
            }
        }

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