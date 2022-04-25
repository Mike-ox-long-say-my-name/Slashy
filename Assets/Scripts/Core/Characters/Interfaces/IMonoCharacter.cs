using Core.Attacking;
using Core.Attacking.Interfaces;
using UnityEngine.Events;

namespace Core.Characters.Interfaces
{
    public interface IMonoCharacter : IMonoHitReceiver
    {
        UnityEvent<ICharacter, ICharacterResource> OnHealthChanged { get; }
        UnityEvent<IHitReceiver, HitInfo> OnHitReceived { get; }
        UnityEvent<ICharacter, HitInfo> OnHitReceivedExclusive { get; }
        UnityEvent<ICharacter, HitInfo> OnStaggered { get; }
        UnityEvent<ICharacter, HitInfo> OnDeath { get; }

        new ICharacter Resolve();
    }
}