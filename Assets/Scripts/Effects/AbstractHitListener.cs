using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters;
using Core.Characters.Mono;
using UnityEngine;

namespace Effects
{
    public abstract class AbstractHitListener : MonoBehaviour
    {
        protected virtual void Subscribe()
        {
            var mixinHittable = GetComponentInParent<MixinHittable>();
            if (mixinHittable == null)
            {
                return;
            }
            mixinHittable.HitReceiver.HitReceived += OnHitReceived;
        }

        protected virtual void Unsubscribe()
        {
            var mixinHittable = GetComponentInParent<MixinHittable>();
            if (mixinHittable == null)
            {
                return;
            }
            mixinHittable.HitReceiver.HitReceived -= OnHitReceived;
        }

        protected abstract void OnHitReceived(IHitReceiver entity, HitInfo info);
    }
}