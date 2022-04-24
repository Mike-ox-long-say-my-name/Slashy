using Core.Characters;
using UnityEngine.Events;

namespace Characters.Player
{
    public interface IMonoPlayerCharacter : IMonoCharacter
    {
        public UnityEvent<IPlayerCharacter, ICharacterResource> OnStaminaChanged { get; }
        new IPlayerCharacter Native { get; }
    }
}