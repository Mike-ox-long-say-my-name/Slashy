using System;
using Attacking;
using UnityEngine;
using UnityEngine.Events;

public abstract class HittableEntity : MonoBehaviour, IHitReceiver
{
    public UnityEvent<HittableEntity> OnHitReceived { get; } = new UnityEvent<HittableEntity>();

    public virtual void ReceiveHit(IHitSource source, in HitInfo info)
    {
        OnHitReceived?.Invoke(this);
    }
}