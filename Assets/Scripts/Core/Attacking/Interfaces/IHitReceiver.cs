using UnityEngine;

namespace Core.Attacking.Interfaces
{
    public interface IHitReceiver
    {
        GameObject Object { get; }

        void ReceiveHit(HitInfo hit);
    }
}