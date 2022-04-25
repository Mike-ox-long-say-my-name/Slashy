using Core.Attacking.Interfaces;
using Core.DependencyInjection;
using Core.Utilities;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MonoAttackbox : MonoBaseHitbox, IMonoAttackbox
    {
        [SerializeField] private bool disableOnAwake = true;
        [SerializeField] private MonoHurtbox[] ignored;

        private IAttackbox _attackbox;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IMonoHurtbox>(out var hit))
            {
                _attackbox.ProcessHit(hit.Resolve());
            }
        }
        
        [AutoResolve]
        public IAttackbox Resolve()
        {
            if (_attackbox != null)
            {
                return _attackbox;
            }

            var receiver = GetComponentInParent<IMonoHitEventReceiver>();
            _attackbox = new Attackbox(transform, receiver, disableOnAwake, Colliders)
            {
                Ignored = ignored.ToNativeList()
            };

            return _attackbox;
        }
    }
}