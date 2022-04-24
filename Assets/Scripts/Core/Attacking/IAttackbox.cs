using UnityEngine;

namespace Core.Attacking
{
    public interface IAttackbox : IHitbox
    {
        void ProcessHit(IHurtbox hit);
        void ClearHits();
    }
}