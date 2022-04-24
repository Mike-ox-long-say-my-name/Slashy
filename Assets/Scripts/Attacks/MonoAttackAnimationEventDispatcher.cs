using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Attacks
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
                receiver.OnEnableHitbox();
            }
        }

        public void OnAnimationShouldDisableHitbox()
        {
            foreach (var receiver in _receivers)
            {
                receiver.OnDisableHitbox();
            }
        }

        public void OnAnimationShouldEndAttack()
        {
            foreach (var receiver in _receivers)
            {
                receiver.OnAnimationEnded();
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