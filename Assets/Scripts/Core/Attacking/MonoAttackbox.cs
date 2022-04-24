using UnityEngine;

namespace Core.Attacking
{
    [DefaultExecutionOrder(-1)]
    public class MonoAttackbox : MonoBaseHitbox, IMonoAttackbox
    {
        [SerializeField] private bool disableOnAwake = true;
        [SerializeField] private MonoHurtbox[] ignored;

        public IAttackbox Attackbox { get; private set; }

        private void Start()
        {
            var receiver = GetComponentInParent<IMonoHitEventReceiver>();
            Attackbox = new Attackbox(transform, receiver, disableOnAwake, Colliders)
            {
                Ignored = ignored.ToNativeList()
            };
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IMonoHurtbox>(out var hit))
            {
                Attackbox.ProcessHit(hit.Native);
            }
        }
    }
}