using System;

namespace Core.Attacking.Interfaces
{
    public interface IHitReceiver
    {
        void ReceiveHit(HitInfo hit);
    }
}