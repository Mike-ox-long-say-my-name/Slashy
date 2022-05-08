using UnityEngine;

namespace Core.Characters.Mono
{
    public class MixinDamageSource : MonoBehaviour
    {
        [SerializeField] private MonoDamageStats damageStats;

        public DamageStats DamageStats => damageStats.DamageStats;
    }
}