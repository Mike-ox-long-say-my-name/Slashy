using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Player.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class MonoPlayerCharacter : MonoBaseCharacter, IMonoPlayerCharacter, IPlayerEventDispatcher
    {
        [SerializeField] private UnityEvent<IPlayerCharacter, ICharacterResource> onStaminaChanged;
        public UnityEvent<IPlayerCharacter, ICharacterResource> OnStaminaChanged => onStaminaChanged;
        
        public IPlayerCharacter Resolve()
        {
            return ((IMonoCharacter)this).Resolve() as IPlayerCharacter;
        }

        [SerializeField] private PlayerStats stats;
        [SerializeField] private PlayerMovementConfig config;

        protected override ICharacter CreateCharacter()
        {
            var controller = GetComponent<CharacterController>();
            var movement = new PlayerMovement(controller, config);
            return new PlayerCharacter(movement, stats, this);
        }

        void IPlayerEventDispatcher.OnStaminaChanged(IPlayerCharacter player, ICharacterResource stamina)
        {
            OnStaminaChanged?.Invoke(player, stamina);
        }
    }
}
