using System;
using Core.Attacking;

namespace Core.Characters.Interfaces
{
    public interface ICharacter
    {
        event Action<ICharacter, HitInfo> HitReceived;
        event Action<ICharacter, HitInfo> Staggered;
        event Action<ICharacter, HitInfo> Dead;

        IResource Health { get; }
        IResource Balance { get; }
        
        bool FreezeHealth { get; set; }
        bool FreezeBalance { get; set; }
        bool CanDie { get; set; }
    }
}