using UnityEngine;

namespace Core.Attacking
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [DefaultExecutionOrder(-2)]
    public class MonoHurtbox : MonoBaseHitbox, IMonoHurtbox
    {
        public IHurtbox Native { get; private set; }

        private void Start()
        {
            var rigidBody = GetComponent<Rigidbody>();
            ConfigureRigidbody(rigidBody);

            var hitReceiver = GetComponentInParent<IMonoHitReceiver>()?.Native;
            Native = new Hurtbox(transform, hitReceiver, Colliders);
        }

        private static void ConfigureRigidbody(Rigidbody rigidBody)
        {
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
    }
}
