using Attacking;
using UnityEngine;
using UnityEngine.Events;

public abstract class HittableEntity : MonoBehaviour, IHitReceiver
{
    [SerializeField] private UnityEvent<HittableEntity, HitInfo> onHitReceived;

    public UnityEvent<HittableEntity, HitInfo> OnHitReceived => onHitReceived;

    public virtual void ReceiveHit(in HitInfo info)
    {
        OnHitReceived?.Invoke(this, info);
    }
}