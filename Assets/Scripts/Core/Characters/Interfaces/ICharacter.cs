using Core.Attacking.Interfaces;

namespace Core.Characters.Interfaces
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