using UnityEngine;

namespace Core.Attacking
{
    public interface IHitReceiver
    {
        GameObject Object { get; }

        void ReceiveHit(HitInfo hit);
    }
}