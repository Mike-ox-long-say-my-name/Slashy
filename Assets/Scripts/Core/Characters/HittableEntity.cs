using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters
{
    public abstract class HittableEntity : MonoBehaviour, IHitSource
    {
        [SerializeField] private UnityEvent<HittableEntity, HitInfo> onHitReceived;

        public UnityEvent<HittableEntity, HitInfo> OnHitReceived => onHitReceived;

        public Transform Transform => transform;
        public HittableEntity Source => this;

        public virtual void ReceiveHit(HitInfo info)
        {
            OnHitReceived?.Invoke(this, info);
        }
    }
}