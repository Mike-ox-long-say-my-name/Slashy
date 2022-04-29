using System;
using UnityEngine;

namespace Core.Attacking.Interfaces
{
    public interface IHitReceiver
    {
        event Action<IHitReceiver, HitInfo> OnHitReceived;

        GameObject Object { get; }

        void ReceiveHit(HitInfo hit);
    }
}