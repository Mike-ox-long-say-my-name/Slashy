using System;
using Core.Characters.Mono;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MixinHitInfoContainer : MonoBehaviour
    {
        [SerializeField] private MonoDamageMultipliers multipliers;

        private MixinDamageSource _damageSource;
        private DamageMultipliers _damageMultipliers;

        public void OverrideMultipliers(DamageMultipliers overridenMultipliers)
        {
            _damageMultipliers = overridenMultipliers;
        }

        private MixinDamageSource GetDamageSource()
        {
            if (_damageSource == null)
            {
                _damageSource = GetComponentInParent<MixinDamageSource>();
                _damageMultipliers = multipliers.DamageMultipliers;
            }

            return _damageSource;
        }

        public HitInfo GetHitInfo()
        {
            var source = GetDamageSource();
            var hitInfo = new HitInfo
            {
                DamageStats = source.DamageStats,
                Multipliers = _damageMultipliers,
                Source = new HitSource
                {
                    IsEnvironmental = source.IsEnvironmental,
                    Character = source.Source,
                    Transform = source.Transform
                }
            };

            return hitInfo;
        }
    }
}