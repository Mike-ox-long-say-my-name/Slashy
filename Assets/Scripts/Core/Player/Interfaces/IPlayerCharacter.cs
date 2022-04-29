using System;
using Core.Characters.Interfaces;

namespace Core.Player.Interfaces
{
    public interface IPlayerCharacter : ICharacter
    {
        event Action<IPlayerCharacter, ICharacterResource> OnStaminaChanged;

        IPlayerMovement PlayerMovement { get; }

        ICharacterResource Stamina { get; }

        void SpendStamina(float amount);
    }
}