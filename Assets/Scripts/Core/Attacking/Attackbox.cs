using System.Collections.Generic;
using Core.Characters;
using UnityEngine;

namespace Core.Attacking
{
    public class Attackbox : BaseHitbox, IAttackbox
    {
        public List<IHurtbox> Ignored { get; set; } = new List<IHurtbox>();

        private readonly HashSet<IHurtbox> _hits = new HashSet<IHurtbox>();

        private readonly IMonoHitEventReceiver _receiver;

        public Attackbox(Transform transform, IMonoHitEventReceiver receiver, bool disable = true, params Collider[] colliders)
            : base(transform, colliders)
        {
            Guard.NotNull(receiver);

            _receiver = receiver;

            if (disable)
            {
                base.Disable();
            }
        }

        public virtual void ClearHits()
        {
            _hits.Clear();
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

            _hits.Add(hit);
            _receiver.OnHit(this, hit);
        }

        protected virtual bool ShouldDispatch(IHurtbox hit)
        {
            return !_hits.Contains(hit) && !Ignored.Contains(hit);
        }
    }
}
