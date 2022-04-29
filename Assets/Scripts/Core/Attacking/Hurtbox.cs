using System;
using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Attacking
{
    public class Hurtbox : BaseHitbox, IHurtbox
    {
        public Hurtbox(Transform transform, params Collider[] colliders) : base(transform, colliders)
        {
        }

        public event Action<HitInfo> OnHit;

        public void ProcessHit(IAttackbox source, HitInfo hitInfo)
        {
            OnHit?.Invoke(hitInfo);
        }
    }
}