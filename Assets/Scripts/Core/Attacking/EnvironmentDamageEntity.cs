using Core.Characters;
using Core.Characters.Mono;
using UnityEngine;

namespace Core.Attacking
{
    public class EnvironmentDamageEntity : MonoBehaviour, IDamageStatsContainer
    {
        [SerializeField] private MonoDamageStats monoDamageStats;

        public DamageStats DamageStats => monoDamageStats.DamageStats;
    }
}