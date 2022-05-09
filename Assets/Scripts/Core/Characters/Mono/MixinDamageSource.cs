using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters.Mono
{
    public class MixinDamageSource : MonoBehaviour
    {
        [SerializeField] private bool hasSource;
        [SerializeField] private bool isEnvironmental;
        [SerializeField] private MonoDamageStats damageStats;

        public DamageStats DamageStats => damageStats.DamageStats;

        private ICharacter _source;

        public ICharacter Source
        {
            get
            {
                if (!hasSource)
                {
                    return null;
                }

                return _source ??= GetComponent<MixinCharacter>().Character;
            }
        }

        public Transform Transform => hasSource ? transform : null;
        public bool IsEnvironmental => isEnvironmental;
    }
}