using Core.Attacking.Interfaces;
using System.Linq;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MonoDotAttackbox : MonoAttackbox
    {
        [SerializeField, Range(0.01f, 5)] private float hitInterval = 1f;
        [SerializeField, Min(0)] private int damageGroup;

        protected override IHitbox CreateHitbox()
        {
            var colliders = GetComponentsInChildren<Collider>();
            var attackbox = new DotAttackbox(transform, damageGroup, colliders)
            {
                HitInterval = hitInterval,
                Ignored = Ignored.Select(hurtbox => hurtbox.Hurtbox).ToList()
            };

            if (DisableOnInit)
            {
                attackbox.Disable();
            }

            return attackbox;
        }

        public new IDotAttackbox Attackbox => Hitbox as IDotAttackbox;

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
            if (other.TryGetComponent<MonoHurtbox>(out var hit))
            {
                Attackbox.ProcessHit(hit.Hurtbox);
            }
        }

        private void Update()
        {
            Attackbox.Tick(Time.deltaTime);
        }
    }
}