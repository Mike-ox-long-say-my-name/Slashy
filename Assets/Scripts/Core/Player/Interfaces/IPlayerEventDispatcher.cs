using Core.Characters.Interfaces;

namespace Core.Player.Interfaces
{
    public interface IPlayerEventDispatcher : ICharacterEventDispatcher
    {
        void OnStaminaChanged(IPlayerCharacter player, ICharacterResource stamina);
    }
}