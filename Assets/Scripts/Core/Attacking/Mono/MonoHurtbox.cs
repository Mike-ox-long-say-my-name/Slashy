using Core.Attacking.Interfaces;
using Core.DependencyInjection;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class MonoHurtbox : MonoBaseHitbox, IMonoHurtbox
    {
        private IHurtbox _hurtbox;

        private void Awake()
        {
            var rigidBody = GetComponent<Rigidbody>();
            ConfigureRigidbody(rigidBody);
        }

        private static void ConfigureRigidbody(Rigidbody rigidBody)
        {
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        
        [AutoResolve]
        public IHurtbox Resolve()
        {
            if (_hurtbox != null)
            {
                return _hurtbox;
            }

            var hitReceiver = GetComponentInParent<IMonoHitReceiver>()?.Resolve();
            _hurtbox = new Hurtbox(transform, hitReceiver, Colliders);

            return _hurtbox;
        }
    }
}
