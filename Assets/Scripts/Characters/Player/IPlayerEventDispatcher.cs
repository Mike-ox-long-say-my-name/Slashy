using Core.Characters;

namespace Characters.Player
{
    public interface IPlayerEventDispatcher : ICharacterEventDispatcher
    {
        void OnStaminaChanged(IPlayerCharacter player, ICharacterResource stamina);
    }
}