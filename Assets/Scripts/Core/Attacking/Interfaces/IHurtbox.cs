using System;
using Core.Characters.Interfaces;

namespace Core.Attacking.Interfaces
{
    public interface IHurtbox : IHitbox
    {
        Team Team { get; }

        event Action<HitInfo> Hit;

        void ProcessHit(IAttackbox source, HitInfo hitInfo);
    }
}