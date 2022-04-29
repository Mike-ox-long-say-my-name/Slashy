using Core.Attacking.Interfaces;
using Core.Characters.Mono;
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

        protected override IHitbox CreateHitbox(Collider[] colliders)
        {
            var character = GetComponentInParent<MonoCharacter>().Character;
            var hurtbox = CreateHurtbox(colliders);
            hurtbox.OnHit += character.ReceiveHit;
            return hurtbox;
        }

        protected virtual IHurtbox CreateHurtbox(Collider[] colliders)
        {
            return new Hurtbox(transform, colliders);
        }
    }
}
