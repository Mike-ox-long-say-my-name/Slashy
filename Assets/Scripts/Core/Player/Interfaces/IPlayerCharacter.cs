using Core.Characters.Interfaces;

namespace Core.Player.Interfaces
{
    public interface IPlayerCharacter : ICharacter
    {
        new IPlayerMovement Movement { get; }

        ICharacterResource Stamina { get; }

        void SpendStamina(float amount);
    }
}