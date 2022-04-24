using Core.Attacking;

namespace Core.Characters
{
    public interface ICharacter : IHitReceiver
    {
        ICharacterMovement Movement { get; }
        IPushable Pushable { get; }

        ICharacterResource Health { get; }
        ICharacterResource Balance { get; }

        void Heal(float amount);
        void Tick(float deltaTime);
    }
}