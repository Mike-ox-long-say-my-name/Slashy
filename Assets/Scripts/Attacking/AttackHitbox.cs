using System;
using System.Collections.Generic;
using UnityEngine;

namespace Attacking
{
    public class AttackHitbox : BaseHitbox
    {
        [SerializeField] private bool disableOnAwake = true;
        [SerializeField] private Hurtbox ignored;

        private Action<Hurtbox> _onHit;

        private readonly Dictionary<int, Hurtbox> _hurtboxes = new Dictionary<int, Hurtbox>();

        protected override void Awake()
        {
            base.Awake();
            if (HitboxCollider != null && disableOnAwake)
            {
                Disable();
            }
        }

        public void ClearHits()
        {
            _hurtboxes.Clear();
        }

        public void EnableWith(Action<Hurtbox> onHit)
        {
            _onHit = onHit;
            Enable();
        }

        public void DisableAndClear()
        {
            Disable();
            ClearHits();
            _onHit = null;
        }

        private void OnTriggerStay(Collider target)
        {
            if (!target.TryGetComponent<Hurtbox>(out var hit))
            {
                return;
            }

            if (ignored != null && target.gameObject == ignored.gameObject)
            {
                return;
            }

            var hitId = hit.gameObject.GetInstanceID();
            if (_hurtboxes.ContainsKey(hitId))
            {
                return;
            }

            _hurtboxes.Add(hitId, hit);
            _onHit?.Invoke(hit);
        }
    }
}
