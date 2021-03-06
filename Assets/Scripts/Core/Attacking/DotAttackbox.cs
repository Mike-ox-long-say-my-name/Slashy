using System;
using Core.Attacking.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Attacking
{
    public sealed class DotAttackbox : BaseHitbox, IDotAttackbox
    {
        private float _hitInterval = 1f;

        public readonly int DamageGroup;

        public float HitInterval
        {
            get => _hitInterval;
            set
            {
                if (value <= 0)
                {
                    Debug.LogWarning("Hit Interval must be positive");
                    return;
                }

                _hitInterval = value;
            }
        }

        private readonly Dictionary<IHurtbox, float> _hitTimes = new Dictionary<IHurtbox, float>();

        private static readonly Dictionary<IHurtbox, HashSet<int>> GlobalHits = new Dictionary<IHurtbox, HashSet<int>>();

        public List<IHurtbox> Ignored { get; set; } = new List<IHurtbox>();

        public DotAttackbox(Transform transform, int damageGroup, params Collider[] colliders) : base(transform, colliders)
        {
            if (damageGroup < 0)
            {
                Debug.LogWarning("Damage Group must be non-negative");
                damageGroup = 0;
            }

            DamageGroup = damageGroup;
        }

        private static void EnsureGlobalHitExist(IHurtbox hit)
        {
            if (!GlobalHits.ContainsKey(hit))
            {
                GlobalHits[hit] = new HashSet<int>();
            }
        }

        public Team Team { get; set; }
        public event Action<IHurtbox> Hit;

        public void ProcessHit(IHurtbox hit)
        {
            EnsureGlobalHitExist(hit);
            if (!ShouldDispatch(hit))
            {
                return;
            }

            _hitTimes.Add(hit, HitInterval);
            GlobalHits[hit].Add(DamageGroup);

            Hit?.Invoke(hit);
        }

        public AttackboxGroup Group { get; set; }

        private void UpdateTimes(float deltaTime)
        {
            var keys = _hitTimes.Keys.ToList();
            foreach (var hit in keys)
            {
                _hitTimes[hit] -= deltaTime;
            }
        }

        private bool ShouldDispatch(IHurtbox hit)
        {
            return IsEnabled && !Ignored.Contains(hit) && !_hitTimes.ContainsKey(hit);
        }

        private void ClearDeadHits()
        {
            var toRemove = _hitTimes
                .Where(pair => pair.Value <= 0)
                .Select(pair => pair.Key)
                .ToList();

            foreach (var removeHit in toRemove)
            {
                _hitTimes.Remove(removeHit);
                GlobalHits[removeHit].Remove(DamageGroup);
            }
        }

        public void ClearHits()
        {
            _hitTimes.Clear();
        }

        public void DisableNoClear()
        {
            Disable();
        }

        public void Tick(float deltaTime)
        {
            UpdateTimes(deltaTime);
            ClearDeadHits();
        }
    }
}