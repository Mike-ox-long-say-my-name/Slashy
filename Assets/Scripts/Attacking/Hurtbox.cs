using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HittableEntity))]
public class Hurtbox : BaseHitbox, IHitReceiver
{
    [SerializeField] private HittableEntity entity;

    public void ReceiveHit(IHitSource source, in HitInfo info)
    {
        entity.ReceiveHit(source, info);
    }
}
