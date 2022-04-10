using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attacking
{
    public interface IHitReceiver
    {
        void ReceiveHit(IHitSource source, in HitInfo info);
    }

    public interface IHitSource
    {
        Transform Transform { get; }
        HittableEntity Source { get; }
    }
}