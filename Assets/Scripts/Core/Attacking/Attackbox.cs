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

        public AttackboxGroup Group { get; set; }

        public Attackbox(Transform transform, params Collider[] colliders) : base(transform, colliders)
        {
        }

        public void ClearHits()
        {
            Group?.Reset();
        }

        public void DisableNoClear()
        {
            base.Disable();
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

            Group ??= new AttackboxGroup();

            if (Group.TryAttack(hit))
            {
                Hit?.Invoke(hit);
            }
        }

        private bool ShouldDispatch(IHurtbox hit)
        {
            return IsEnabled && (Team == Team.None || hit.Team != Team) && !Ignored.Contains(hit);
        }
    }
}
