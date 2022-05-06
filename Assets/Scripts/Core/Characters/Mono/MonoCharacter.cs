using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters.Mono
{
    [RequireComponent(typeof(CharacterController))]
    public class MonoCharacter : MonoBehaviour, IDamageStatsContainer, IHitReceiver, IHitReceiveDispatcher
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
                _character.OnHitReceived += HitReceived.Invoke;
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

        public void ReceiveHit(HitInfo hit) => Character.ReceiveHit(hit);

        public UnityEvent<IHitReceiver, HitInfo> HitReceived { get; } = new UnityEvent<IHitReceiver, HitInfo>();
    }
}