using Core.Characters;

namespace Characters.Player
{
    public interface IPlayerCharacter : ICharacter
    {
        new IPlayerMovement Movement { get; }

        ICharacterResource Stamina { get; }

        void SpendStamina(float amount);
    }
}