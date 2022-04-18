using UnityEngine;

namespace Attacking
{
    [RequireComponent(typeof(Rigidbody))]
    public class Hurtbox : BaseHitbox, IHitReceiver
    {
        [SerializeField] private HittableEntity entity;

        public void ReceiveHit(in HitInfo info)
        {
            if (entity)
            {
                entity.ReceiveHit(info);
            }
        }
    }
}
