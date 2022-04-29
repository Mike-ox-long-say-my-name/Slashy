using Core.Attacking.Interfaces;
using Core.Characters.Mono;
using System.Collections.Generic;
using System.Linq;
using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;
using UnityEngine.Video;

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
            DamageStats damageStats;
            ICharacter source;

            var monoCharacter = GetComponentInParent<MonoCharacter>();
            if (monoCharacter == null)
            {
                var container = GetComponentInParent<IDamageStatsContainer>();
                damageStats = container.DamageStats;
                source = null;
            }
            else
            {
                source = monoCharacter.Character;
                damageStats = source.DamageStats;
            }
            
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