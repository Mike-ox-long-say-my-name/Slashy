using Core.Characters.Interfaces;
using Core.Modules;
using UnityEngine;

namespace Core.Characters.Mono
{
    [RequireComponent(typeof(MixinHealth))]
    [RequireComponent(typeof(MixinBalance))]
    public class MixinCharacter : MonoBehaviour
    {
        [SerializeField] private bool canDie = true;

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


                _character = new Character(health, balance, hitReceiver)
                {
                    CanDie = canDie
                };
                return _character;
            }
        }
    }
}