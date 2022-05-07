using Core.Attacking.Interfaces;
using Core.Characters.Mono;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MonoAttackbox : MonoBaseHitbox
    {
        [SerializeField] private bool disableOnInit = true;
        [SerializeField] private MonoHurtbox[] ignored;

        [SerializeField] private bool isEnvironmental;
        [SerializeField] private MonoDamageMultipliers monoDamageMultipliers;

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

        protected virtual IAttackbox CreateAttackbox(Collider[] colliders, HitInfo hitInfo)
        {
            var attackbox = new Attackbox(transform, colliders)
            {
                Ignored = ignored != null ? ignored.Select(hurtbox => hurtbox.Hurtbox).ToList() : new List<IHurtbox>(),
                HitInfo = hitInfo
            };

            if (disableOnInit)
            {
                attackbox.Disable();
            }

            return attackbox;
        }

        protected override IHitbox CreateHitbox(Collider[] colliders)
        {
            var damageStats = GetComponentInParent<MixinDamageSource>().DamageStats;
            var mixinCharacter = GetComponentInParent<MixinCharacter>();
            var source = mixinCharacter != null ? mixinCharacter.Character : null;

            return CreateAttackbox(colliders, new HitInfo
            {
                Multipliers = monoDamageMultipliers.DamageMultipliers,
                DamageStats = damageStats,
                Source = new HitSource
                {
                    IsEnvironmental = isEnvironmental,
                    Character = source
                }
            });
        }
    }
}