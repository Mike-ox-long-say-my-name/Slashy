using Core.Attacking;
using Core.Attacking.Interfaces;
using UnityEngine.Events;

namespace Core
{
    public interface IHitReceiveDispatcher
    {
        UnityEvent<IHitReceiver, HitInfo> HitReceived { get; }
    }
}