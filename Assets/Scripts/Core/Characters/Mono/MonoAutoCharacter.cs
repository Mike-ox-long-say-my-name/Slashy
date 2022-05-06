using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters.Mono
{
    public class MonoAutoCharacter : MonoCharacter
    {
        protected override ICharacter CreateCharacter(MovementConfig config, DamageStats damageState, CharacterStats characterStats)
        {
            var controller = GetComponent<CharacterController>();

            var rawMovement = new Movement(controller);

            var movement = new AutoMovement(rawMovement, config);
            return new AutoCharacter(movement, damageState, characterStats);
        }

    }
}