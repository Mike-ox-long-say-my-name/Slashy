using System;
using Core.Attacking;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.States
{
    public class PurityDamageScaler : MonoBehaviour
    {
        [Serializable]
        private struct HitInfoContainerData
        {
            public MixinHitInfoContainer container;
            public DamageMultipliers minMultipliers;
            public DamageMultipliers MaxMultipliers { get; set; }
        }

        [SerializeField] private HitInfoContainerData[] overrideData;

        private void Awake()
        {
            for (var i = 0; i < overrideData.Length; i++)
            {
                overrideData[i].MaxMultipliers = overrideData[i].container.GetHitInfo().Multipliers;
            }
        }

        public void OverrideMultipliers(IResource purity)
        {
            var fraction = purity.Value / purity.MaxValue;

            foreach (var containerData in overrideData)
            {
                var container = containerData.container;
                var min = containerData.minMultipliers;
                var max = containerData.MaxMultipliers;
                var newMultipliers = new DamageMultipliers
                {
                    DamageMultiplier =
                        Mathf.Lerp(min.DamageMultiplier, max.DamageMultiplier, fraction),
                    BalanceDamageMultiplier =
                        Mathf.Lerp(min.BalanceDamageMultiplier, max.BalanceDamageMultiplier, fraction),
                    PushMultiplier =
                        Mathf.Lerp(min.PushMultiplier, max.PushMultiplier, fraction),
                    StaggerTimeMultiplier =
                        Mathf.Lerp(min.StaggerTimeMultiplier, max.StaggerTimeMultiplier, fraction)
                };
                container.OverrideMultipliers(newMultipliers);
            }
        }
    }
}