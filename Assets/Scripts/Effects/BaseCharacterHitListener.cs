using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using UnityEngine;

namespace Effects
{
    public abstract class BaseCharacterHitListener : MonoBehaviour
    {
        protected virtual void Subscribe()
        {
            var receiver = GetComponentInParent<IHitReceiveDispatcher>();
            receiver?.HitReceived.AddListener(OnHitReceived);
        }

        protected virtual void Unsubscribe()
        {
            var receiver = GetComponentInParent<IHitReceiveDispatcher>();
            receiver?.HitReceived.RemoveListener(OnHitReceived);
        }

        protected abstract void OnHitReceived(IHitReceiver entity, HitInfo info);
    }
}