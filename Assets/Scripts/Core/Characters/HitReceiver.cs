using System;
using Core.Attacking;
using Core.Attacking.Interfaces;

namespace Core.Characters
{
    public class HitReceiver : IHitReceiver
    {
        public event Action<IHitReceiver, HitInfo> HitReceived;

        public void ReceiveHit(HitInfo hit)
        {
            HitReceived?.Invoke(this, hit);
        }
    }
}