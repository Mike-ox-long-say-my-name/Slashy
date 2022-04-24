using System;
using System.Collections.Generic;
using System.Linq;
using Core.Characters;
using UnityEngine;

namespace Core.Attacking
{
    public class DotAttackbox : BaseHitbox, IDotAttackbox
    {
        private int _damageGroup;
        private float _hitInterval = 1f;

        public int DamageGroup
        {
            get => _damageGroup;
            set
            {
                if (value < 0)
                {
                    return;
                }

                _damageGroup = value;
            }
        }

        public float HitInterval
        {
            get => _hitInterval;
            set
            {
                if (value <= 0)
                {
                    return;
                }

                _hitInterval = value;
            }
        }

        public List<IHurtbox> Ignored { get; set; } = new List<IHurtbox>();

        private readonly Dictionary<IHurtbox, float> _hits = new Dictionary<IHurtbox, float>();
        private readonly IMonoHitEventReceiver _receiver;

        public DotAttackbox(Transform transform, IMonoHitEventReceiver receiver, bool disable = true, params Collider[] colliders)
            : base(transform, colliders)
        {
            Guard.NotNull(receiver);

            _receiver = receiver;
            if (disable)
            {
                base.Disable();
            }
        }

        public void ProcessHit(IHurtbox hit)
        {
            if (!ShouldDispatch(hit))
            {
                return;
            }

            _hits.Add(hit, _hitInterval);
            _receiver.OnHit(this, hit);
        }

        private bool ShouldDispatch(IHurtbox hit)
        {
            return !_hits.ContainsKey(hit) && !Ignored.Contains(hit);
        }

        private void UpdateTimes(float deltaTime)
        {
            var keys = _hits.Keys.ToList();
            foreach (var hit in keys)
            {
                _hits[hit] -= deltaTime;
            }
        }

        private void ClearDeadHits()
        {
            var toRemove = _hits
                .Where(pair => pair.Value <= 0)
                .Select(pair => pair.Key)
                .ToList();

            foreach (var removeHit in toRemove)
            {
                _hits.Remove(removeHit);
            }
        }

        public void Tick(float deltaTime)
        {
            UpdateTimes(deltaTime);
            ClearDeadHits();
        }
    }
}