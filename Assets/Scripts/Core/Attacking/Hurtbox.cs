using System;
using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Attacking
{
    public class Hurtbox : BaseHitbox, IHurtbox
    {
        public IHitReceiver AttachedTo { get; }

        public Hurtbox(Transform transform, IHitReceiver receiver, params Collider[] colliders) : base(transform, colliders)
        {
            AttachedTo = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        public void Dispatch(HitInfo info)
        {
            AttachedTo.ReceiveHit(info);
        }
    }
}