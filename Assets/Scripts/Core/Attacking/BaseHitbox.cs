using System;
using System.Linq;
using Core.Characters;
using UnityEngine;

namespace Core.Attacking
{
    public class BaseHitbox : IHitbox
    {
        public Transform Transform { get; }

        private readonly Collider[] _colliders;

        public BaseHitbox(Transform transform, params Collider[] colliders)
        {
            Guard.NotNull(transform);

            Transform = transform;
            _colliders = colliders != null ? colliders.ToArray() : Array.Empty<Collider>();
        }

        public bool IsEnabled { get; private set; }

        public virtual void Enable()
        {
            foreach (var hitbox in _colliders)
            {
                hitbox.enabled = true;
            }

            IsEnabled = true;
        }

        public virtual void Disable()
        {
            foreach (var hitbox in _colliders)
            {
                hitbox.enabled = false;
            }
            IsEnabled = false;
        }
    }
}