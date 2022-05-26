using System;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Modules;

namespace Core.Characters.Interfaces
{
    public interface ICharacter : IUpdateable
    {
        Team Team { get; set; }

        event Action<HitInfo> HitReceived;
        event Action<HitInfo> Staggered;
        event Action<HitInfo> Dead;
        event Action RecoveredFromStagger;

        IResource Health { get; }
        IResource Balance { get; }
        IHitReceiver HitReceiver { get; }

        bool CanDie { get; set; }
        void Kill();
    }
}