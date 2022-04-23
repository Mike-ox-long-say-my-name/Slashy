using Core;
using Core.Characters;
using UnityEngine;

namespace Attacking
{
    [RequireComponent(typeof(Rigidbody))]
    public class Hurtbox : BaseHitbox
    {
        [SerializeField] private HittableEntity entity;

        protected override void Awake()
        {
            if (entity == null)
            {
                Debug.LogWarning("Entity is not assigned", this);
                enabled = false;
            }
        }

        public void ReceiveHit(HitInfo info)
        {
            entity.ReceiveHit(info);
        }
    }
}
