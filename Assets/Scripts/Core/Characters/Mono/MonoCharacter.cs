using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters.Mono
{
    [RequireComponent(typeof(CharacterController))]
    public class MonoCharacter : MonoBehaviour, IDamageStatsContainer, IHitReceiver
    {
        [SerializeField] private MonoMovementConfig monoMovementConfig;
        [SerializeField] private MonoCharacterStats monoCharacterStats;
        [SerializeField] private MonoDamageStats monoDamageStats;

        public DamageStats DamageStats => monoDamageStats.DamageStats;

        private ICharacter _character;

        public ICharacter Character
        {
            get
            {
                if (_character != null)
                {
                    return _character;
                }

                _character = CreateCharacter(monoMovementConfig.Config, monoDamageStats.DamageStats, monoCharacterStats.CharacterStats);
                return _character;
            }
        }

        protected virtual void Update()
        {
            Character.Tick(Time.deltaTime);
        }

        protected virtual ICharacter CreateCharacter(MovementConfig config, DamageStats damageState, CharacterStats characterStats)
        {
            var controller = GetComponent<CharacterController>();

            var rawMovement = new Movement(controller);

            var movement = new CharacterMovement(rawMovement, config);
            return new Character(movement, damageState, characterStats);
        }

        public void ReceiveHit(HitInfo hit)
        {
            Character.ReceiveHit(hit);
        }
    }
}