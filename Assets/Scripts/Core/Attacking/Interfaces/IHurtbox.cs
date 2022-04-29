using System;

namespace Core.Attacking.Interfaces
{
    public interface IHurtbox : IHitbox
    {
        event Action<HitInfo> OnHit;

        void ProcessHit(IAttackbox source, HitInfo hitInfo);
    }
}