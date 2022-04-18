using Core;
using Core.Characters;
using UnityEngine;

namespace Attacking
{
    [RequireComponent(typeof(Rigidbody))]
    public class Hurtbox : BaseHitbox
    {
        [SerializeField] private HittableEntity entity;

        public void ReceiveHit(HitInfo info)
        {
            if (entity != null)
            {
                entity.ReceiveHit(info);
            }
        }
    }
}
