using Core.Attacking.Interfaces;
using System;
using System.Collections.Generic;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Attacking
{
    public class Attackbox : BaseHitbox, IAttackbox
    {
        public List<IHurtbox> Ignored { get; set; } = new List<IHurtbox>();

        private readonly HashSet<IHurtbox> _hits = new HashSet<IHurtbox>();

        public Attackbox(Transform transform, params Collider[] colliders) : base(transform, colliders)
        {
        }

        public void ClearHits()
        {
            _hits.Clear();
        }

        public override void Disable()
        {
            ClearHits();
            base.Disable();
        }

        public Team Team { get; set; }
        public event Action<IHurtbox> Hit;

        public void ProcessHit(IHurtbox hit)
        {
            if (!ShouldDispatch(hit))
            {
                return;
            }

            _hits.Add(hit);
            Hit?.Invoke(hit);
        }

        private bool ShouldDispatch(IHurtbox hit)
        {
            return IsEnabled && (Team == Team.None || hit.Team != Team) && !_hits.Contains(hit) && !Ignored.Contains(hit);
        }
    }
}
