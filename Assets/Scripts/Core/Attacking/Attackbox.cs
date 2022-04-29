using Core.Attacking.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Attacking
{
    public class Attackbox : BaseHitbox, IAttackbox
    {
        public List<IHurtbox> Ignored { get; set; } = new List<IHurtbox>();

        protected readonly HashSet<IHurtbox> Hits = new HashSet<IHurtbox>();

        public event Action<IAttackbox, IHurtbox> OnHit;

        public Attackbox(Transform transform, params Collider[] colliders) : base(transform, colliders)
        {
        }

        public virtual void ClearHits()
        {
            Hits.Clear();
        }

        public override void Disable()
        {
            ClearHits();
            base.Disable();
        }

        public virtual void ProcessHit(IHurtbox hit)
        {
            if (!ShouldDispatch(hit))
            {
                return;
            }

            Hits.Add(hit);
            OnHit?.Invoke(this, hit);
        }

        protected virtual bool ShouldDispatch(IHurtbox hit)
        {
            return !Hits.Contains(hit) && !Ignored.Contains(hit);
        }
    }
}
