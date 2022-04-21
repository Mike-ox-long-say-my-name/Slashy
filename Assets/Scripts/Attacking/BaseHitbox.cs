using System;
using UnityEngine;

namespace Attacking
{
    [RequireComponent(typeof(Collider))]
    public class BaseHitbox : MonoBehaviour
    {
        [SerializeField] private Collider hitboxCollider;

        public bool IsEnabled => hitboxCollider.enabled;
        public Collider HitboxCollider => hitboxCollider;

        protected virtual void Awake()
        {
            if (hitboxCollider == null)
            {
                Debug.LogWarning("Hitbox Collider is not assigned", this);
                enabled = false;
            }
        }

        public virtual void Enable()
        {
            hitboxCollider.enabled = true;
        }

        public virtual void Disable()
        {
            hitboxCollider.enabled = false;
        }
    }
}
