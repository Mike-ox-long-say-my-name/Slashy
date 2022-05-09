using System;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Attacking
{
    public class Hurtbox : BaseHitbox, IHurtbox
    {
        public Hurtbox(Transform transform, params Collider[] colliders) : base(transform, colliders)
        {
        }

        public Team Team { get; set; }
        public event Action<HitInfo> Hit;

        public void ProcessHit(IAttackbox source, HitInfo hitInfo)
        {
            Hit?.Invoke(hitInfo);
        }
    }
}