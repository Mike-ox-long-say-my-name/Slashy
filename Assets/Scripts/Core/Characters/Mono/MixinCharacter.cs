using Core.Attacking;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Modules;
using UnityEngine;

namespace Core.Characters.Mono
{
    [RequireComponent(typeof(MixinHealth))]
    [RequireComponent(typeof(MixinBalance))]
    [RequireComponent(typeof(MixinHittable))]
    [RequireComponent(typeof(MixinTeam))]
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
                var team = GetComponent<MixinTeam>().Team;

                _character = new Character(health, balance, hitReceiver)
                {
                    CanDie = canDie,
                    Team = team
                };
                return _character;
            }
        }

        private void Update()
        {
            Character.Tick(Time.deltaTime);
        }
    }
}