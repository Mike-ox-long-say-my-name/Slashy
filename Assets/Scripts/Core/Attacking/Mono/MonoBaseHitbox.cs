using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public abstract class MonoBaseHitbox : MonoBehaviour
    {
        [SerializeField] private Collider[] colliders;

        private IHitbox _hitbox;

        public IHitbox Hitbox
        {
            get
            {
                if (_hitbox != null)
                {
                    return _hitbox;
                }

                _hitbox = CreateHitbox(colliders);
                return _hitbox;
            }
        }

        protected abstract IHitbox CreateHitbox(Collider[] colliders);
    }
}