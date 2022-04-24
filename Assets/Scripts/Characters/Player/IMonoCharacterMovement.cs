using Core.Characters;

namespace Characters.Player
{
    public interface IMonoCharacterMovement
    {
        ICharacterMovement Movement { get; }
    }
}