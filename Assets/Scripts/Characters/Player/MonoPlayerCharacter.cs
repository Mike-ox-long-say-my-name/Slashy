using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class MonoPlayerCharacter : MonoBaseCharacter
    {
        [SerializeField] private PlayerStats stats;
        [SerializeField] private PlayerMovementConfig config;

        protected override ICharacter CreateCharacter()
        {
            var controller = GetComponent<CharacterController>();
            var movement = new PlayerMovement(controller, config);
            return new PlayerCharacter(movement, stats);
        }

        public IPlayerCharacter Player => Character as IPlayerCharacter;
    }
}
