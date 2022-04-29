using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class MonoHurtbox : MonoBaseHitbox
    {
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

        public IHurtbox Hurtbox => Hitbox as IHurtbox;

        protected override IHitbox CreateHitbox()
        {
            return new Hurtbox(transform, Colliders);
        }
    }
}
