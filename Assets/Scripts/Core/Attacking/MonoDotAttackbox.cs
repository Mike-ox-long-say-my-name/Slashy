using UnityEngine;

namespace Core.Attacking
{
    public class MonoDotAttackbox : MonoBaseHitbox, IMonoDotAttackbox
    {
        [SerializeField] private bool disableOnAwake = false;
        [SerializeField, Range(0.01f, 5)] private float hitInterval = 1f;
        [SerializeField, Min(0)] private int damageGroup;

        [SerializeField] private MonoHurtbox[] ignored;

        public IDotAttackbox Attackbox { get; private set; }

        private void Start()
        {
            var receiver = GetComponentInParent<IMonoHitEventReceiver>();
            var colliders = GetComponentsInChildren<Collider>();

            Attackbox = new DotAttackbox(transform, receiver, disableOnAwake, colliders)
            {
                HitInterval = hitInterval,
                DamageGroup = damageGroup,
                Ignored = ignored.ToNativeList()
            };
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
                Attackbox.ProcessHit(hit.Native);
            }
        }

        private void Update()
        {
            Attackbox.Tick(Time.deltaTime);
        }
    }
}