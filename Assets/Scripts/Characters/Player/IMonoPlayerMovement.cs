using Core.Characters;

namespace Characters.Player
{
    public interface IMonoPlayerMovement : IMonoCharacterMovement
    {
        new IPlayerMovement Movement { get; }
    }
}