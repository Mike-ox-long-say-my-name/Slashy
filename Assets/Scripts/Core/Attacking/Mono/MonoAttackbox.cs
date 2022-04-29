using Core.Attacking.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MonoAttackbox : MonoBaseHitbox
    {
        [SerializeField] private bool disableOnInit = true;
        [SerializeField] private MonoHurtbox[] ignored;

        protected bool DisableOnInit => disableOnInit;
        protected IEnumerable<MonoHurtbox> Ignored => ignored;

        private void OnTriggerEnter(Collider other)
        {
            ProcessTrigger(other);
        }

        private void OnTriggerStay(Collider other)
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

        public IAttackbox Attackbox => Hitbox as IAttackbox;

        protected override IHitbox CreateHitbox()
        {
            var attackbox = new Attackbox(transform, Colliders)
            {
                Ignored = ignored.Select(hurtbox => hurtbox.Hurtbox).ToList()
            };

            if (disableOnInit)
            {
                attackbox.Disable();
            }
            
            return attackbox;
        }
    }
}