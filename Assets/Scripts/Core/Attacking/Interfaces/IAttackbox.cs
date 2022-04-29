using System;

namespace Core.Attacking.Interfaces
{
    public interface IAttackbox : IHitbox
    {
        event Action<IAttackbox, IHurtbox> OnHit; 

        void ProcessHit(IHurtbox hit);
    }
}