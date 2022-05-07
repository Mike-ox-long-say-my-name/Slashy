using System;

namespace Core.Attacking.Interfaces
{
    public interface IHitReceiver
    {
        event Action<IHitReceiver, HitInfo> HitReceived;

        void ReceiveHit(HitInfo hit);
    }
}