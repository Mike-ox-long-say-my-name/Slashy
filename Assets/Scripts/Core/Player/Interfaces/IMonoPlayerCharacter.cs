using Core.Characters.Interfaces;
using UnityEngine.Events;

namespace Core.Player.Interfaces
{
    public interface IMonoPlayerCharacter : IMonoCharacter
    {
        UnityEvent<IPlayerCharacter, ICharacterResource> OnStaminaChanged { get; }
        new IPlayerCharacter Resolve();
    }
}