using Core.Characters;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class MonoPlayerCharacter : MonoBaseCharacter, IMonoPlayerCharacter, IPlayerEventDispatcher
    {
        [SerializeField] private UnityEvent<IPlayerCharacter, ICharacterResource> onStaminaChanged;
        public UnityEvent<IPlayerCharacter, ICharacterResource> OnStaminaChanged => onStaminaChanged;

        [SerializeField] private PlayerStats stats;
        [SerializeField] private PlayerMovementConfig config;

        IPlayerCharacter IMonoPlayerCharacter.Native => Native as IPlayerCharacter;

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
