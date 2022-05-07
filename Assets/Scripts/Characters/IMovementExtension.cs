using Core.Characters.Interfaces;

namespace Characters
{
    public interface IMovementExtension
    {
        IVelocityMovement Movement { get; }
    }
}