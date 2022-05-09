using System.Collections.Generic;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MixinColliderStorage : MonoBehaviour
    {
        private Collider[] _colliders;

        public IReadOnlyList<Collider> GetColliders()
        {
            if (_colliders != null)
            {
                return _colliders;
            }

            _colliders = GetComponents<Collider>();
            return _colliders;
        }
    }
}