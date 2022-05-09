using Core.Characters.Interfaces;

namespace Core.Player.Interfaces
{
    public interface IPlayerCharacter
    {
        ICharacter Character { get; }
        IResource Stamina { get; }
    }
}