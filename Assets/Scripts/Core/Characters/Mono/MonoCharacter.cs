using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters.Mono
{
    [RequireComponent(typeof(CharacterController))]
    public class MonoCharacter : MonoBaseCharacter
    {
        [SerializeField] private CharacterMovementConfig movementConfig;
        [SerializeField] private CharacterStats characterStats;

        protected override ICharacter CreateCharacter()
        {
            var controller = GetComponent<CharacterController>();

            var movement = new CharacterMovement(controller, movementConfig);
            var pushable = new Pushable(movement);

            return new Character(movement, pushable, characterStats);
        }
    }
}