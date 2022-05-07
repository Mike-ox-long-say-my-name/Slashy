using Core.Characters.Interfaces;
using Core.Modules;
using UnityEngine;

namespace Core.Characters.Mono
{
    public abstract class MixinResource : MonoBehaviour
    {
        [SerializeField] private float maxValue;

        public float MaxValue => maxValue;

        public abstract IResource Resource { get; }
    }

    public class MixinDamageSource : MonoBehaviour
    {
        [SerializeField] private MonoDamageStats damageStats;

        public DamageStats DamageStats => damageStats.DamageStats;
    }

    [RequireComponent(typeof(MixinHealth))]
    [RequireComponent(typeof(MixinBalance))]
    public class MixinCharacter : MonoBehaviour
    {
        [SerializeField] private MonoCharacterStats monoCharacterStats;

        private ICharacter _character;

        public ICharacter Character
        {
            get
            {
                if (_character != null)
                {
                    return _character;
                }

                var health = GetComponent<MixinHealth>().Health;
                var balance = GetComponent<MixinBalance>().Balance;
                var hitReceiver = GetComponent<MixinHittable>().HitReceiver;


                _character = new Character(health, balance, hitReceiver, monoCharacterStats.CharacterStats);
                return _character;
            }
        }
    }
}