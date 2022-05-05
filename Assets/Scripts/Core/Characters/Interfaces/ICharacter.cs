using System;
using Core.Attacking;
using Core.Attacking.Interfaces;

namespace Core.Characters.Interfaces
{
    public interface ICharacter : IHitReceiver
    {
        event Action<IHitReceiver, HitInfo> OnHitReceived;
        event Action<ICharacter, HitInfo> OnHitReceivedExclusive;
        event Action<ICharacter, HitInfo> OnStaggered;
        event Action<ICharacter, HitInfo> OnDeath;

        IVelocityMovement VelocityMovement { get; }

        ICharacterResource Health { get; }
        ICharacterResource Balance { get; }

        CharacterStats CharacterStats { get; set; }
        DamageStats DamageStats { get; set; }
        
        void Heal(float amount);
        void Tick(float deltaTime);
    }
}