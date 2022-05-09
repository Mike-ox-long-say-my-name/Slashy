using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.Modules;
using UnityEngine;

namespace Core.Characters
{
    [RequireComponent(typeof(MixinMovementBase))]
    public class MixinPushable : MonoBehaviour
    {
        private IPushable _pushable;

        public IPushable Pushable
        {
            get
            {
                if (_pushable != null)
                {
                    return _pushable;
                }

                var movement = GetComponent<MixinMovementBase>().BaseMovement;
                _pushable = new Pushable(movement);
                TrySubscribeToHittable();

                return _pushable;
            }
        }

        private void TrySubscribeToHittable()
        {
            var mixinHittable = GetComponent<MixinHittable>();
            if (mixinHittable == null)
            {
                return;
            }

            mixinHittable.HitReceiver.HitReceived += OnHitReceived;
        }

        private void OnHitReceived(IHitReceiver receiver, HitInfo info)
        {
            var sourceTransform = info.Source.Transform;
            if (sourceTransform == null)
            {
                return;
            }
            var direction = (transform.position - sourceTransform.position);
            direction.y = 0;
            Pushable.Push(direction.normalized, info.PushForce, info.PushTime);
        }

        private void Update()
        {
            Pushable.Tick(Time.deltaTime);
        }
    }
}