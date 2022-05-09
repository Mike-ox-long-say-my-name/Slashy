using Core.Characters.Mono;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MixinHitInfoContainer : MonoBehaviour
    {
        [SerializeField] private MonoDamageMultipliers multipliers;

        private MixinDamageSource _damageSource;

        private MixinDamageSource GetDamageSource()
        {
            if (_damageSource == null)
            {
                _damageSource = GetComponentInParent<MixinDamageSource>();
            }

            return _damageSource;
        }

        public HitInfo GetHitInfo()
        {
            var source = GetDamageSource();
            var hitInfo = new HitInfo
            {
                DamageStats = source.DamageStats,
                Multipliers = multipliers.DamageMultipliers,
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