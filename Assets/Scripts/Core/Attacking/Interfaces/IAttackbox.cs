using System;
using Core.Characters.Interfaces;

namespace Core.Attacking.Interfaces
{
    public interface IAttackbox : IHitbox
    {
        Team Team { get; }

        event Action<IHurtbox> Hit;

        void ProcessHit(IHurtbox hit);
    }
}