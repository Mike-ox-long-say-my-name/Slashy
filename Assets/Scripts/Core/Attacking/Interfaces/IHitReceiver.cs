using System;

namespace Core.Attacking.Interfaces
{
    public interface IHitReceiver
    {
        event Action<IHitReceiver, HitInfo> OnHitReceived;

        void ReceiveHit(HitInfo hit);
    }
}