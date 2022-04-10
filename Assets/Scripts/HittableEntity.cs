using System;
using Attacking;
using UnityEngine;

public abstract class HittableEntity : MonoBehaviour, IHitReceiver
{
    public abstract void ReceiveHit(IHitSource source, in HitInfo info);
}