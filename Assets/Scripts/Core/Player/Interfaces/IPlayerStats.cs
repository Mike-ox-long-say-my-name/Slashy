using Core.Characters.Interfaces;

namespace Core.Player.Interfaces
{
    public interface IPlayerStats : ICharacterStats
    {
        bool FreezeStamina { get; }
        float MaxStamina { get; }
        float StaminaRegeneration { get; }
        float StaminaRegenerationDelay { get; }
        float EmptyStaminaAdditionalRegenerationDelay { get; }
    }
}