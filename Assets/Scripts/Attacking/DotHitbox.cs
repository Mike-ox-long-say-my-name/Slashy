using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Attacking
{
    public class DotHitbox : BaseHitbox
    {
        [SerializeField] private UnityEvent<Hurtbox> onHit;
        public UnityEvent<Hurtbox> OnHit => onHit;

        [Range(0.01f, 5)] public float hitInterval = 1f;
        [Min(0)] private int damageGroup;
        [SerializeField] private List<Hurtbox> ignored = new List<Hurtbox>();

        private readonly Dictionary<Hurtbox, float> _hits = new Dictionary<Hurtbox, float>();

        private void OnTriggerStay(Collider other)
        {
            ProcessTrigger(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            ProcessTrigger(other);
        }

        private void ProcessTrigger(Collider trigger)
        {
            if (!trigger.TryGetComponent<Hurtbox>(out var hit))
            {
                return;
            }

            if (ignored.Contains(hit))
            {
                return;
            }

            if (_hits.ContainsKey(hit))
            {
                return;
            }

            _hits.Add(hit, hitInterval);
            OnHit?.Invoke(hit);
        }

        private void LateUpdate()
        {
            UpdateTimes();
            ClearDeadHits();
        }

        private void UpdateTimes()
        {
            var keys = _hits.Keys.ToList();
            foreach (var hit in keys)
            {
                _hits[hit] -= Time.deltaTime;
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
    }
}