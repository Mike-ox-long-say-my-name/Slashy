using System;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Modules;

namespace Core.Characters.Interfaces
{
    public interface ICharacter : IUpdateable
    {
        Team Team { get; set; }

        event Action<ICharacter, HitInfo> HitReceived;
        event Action<ICharacter, HitInfo> Staggered;
        event Action<ICharacter> RecoveredFromStagger;
        event Action<ICharacter, HitInfo> Dead;

        IResource Health { get; }
        IResource Balance { get; }
        IHitReceiver HitReceiver { get; }

        bool CanDie { get; set; }
    }
}