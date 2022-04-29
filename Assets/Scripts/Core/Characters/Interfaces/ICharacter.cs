using System;
using Core.Attacking;
using Core.Attacking.Interfaces;

namespace Core.Characters.Interfaces
{
    public interface ICharacter : IHitReceiver
    {
        event Action<ICharacter, ICharacterResource> OnHealthChanged;
        event Action<ICharacter, HitInfo> OnHitReceivedExclusive;
        event Action<ICharacter, HitInfo> OnStaggered;
        event Action<ICharacter, HitInfo> OnDeath;

        ICharacterMovement Movement { get; }
        IPushable Pushable { get; }

        ICharacterResource Health { get; }
        ICharacterResource Balance { get; }

        void Heal(float amount);
        void Tick(float deltaTime);
    }
}