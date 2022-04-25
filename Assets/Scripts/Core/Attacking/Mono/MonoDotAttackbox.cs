using Core.Attacking.Interfaces;
using Core.DependencyInjection;
using Core.Utilities;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MonoDotAttackbox : MonoBaseHitbox, IMonoDotAttackbox
    {
        [SerializeField] private bool disableOnAwake = false;
        [SerializeField, Range(0.01f, 5)] private float hitInterval = 1f;
        [SerializeField, Min(0)] private int damageGroup;

        [SerializeField] private MonoHurtbox[] ignored;

        private IDotAttackbox _attackbox;
        
        [AutoResolve]
        public IDotAttackbox Resolve()
        {
            if (_attackbox != null)
            {
                return _attackbox;
            }
            
            var receiver = GetComponentInParent<IMonoHitEventReceiver>();
            var colliders = GetComponentsInChildren<Collider>();

            _attackbox = new DotAttackbox(transform, receiver, disableOnAwake, colliders)
            {
                HitInterval = hitInterval,
                DamageGroup = damageGroup,
                Ignored = ignored.ToNativeList()
            };

            return _attackbox;
        }

        private void OnTriggerStay(Collider other)
        {
            ProcessTrigger(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            ProcessTrigger(other);
        }

        private void ProcessTrigger(Component other)
        {
            if (other.TryGetComponent<IMonoHurtbox>(out var hit))
            {
                if (_attackbox == null)
                {

                }
                _attackbox.ProcessHit(hit.Resolve());
            }
        }

        private void Update()
        {
            _attackbox.Tick(Time.deltaTime);
        }
    }
}