using Core;
using UnityEngine;

namespace Attacking
{
    public class DotAttackExecutor : MonoBehaviour
    {
        [SerializeField] private DotHitbox hitbox;

        public DamageInfo damageInfo;

        private void Awake()
        {
            if (hitbox == null)
            {
                Debug.LogWarning("Dot Hitbox is not assigned", this);
                enabled = false;
            }
            else
            {
                hitbox.OnHit.AddListener(OnHit);
            }
        }

        private void OnHit(Hurtbox hit)
        {
            hit.ReceiveHit(new HitInfo
            {
                DamageInfo = damageInfo,
                HitSource = null
            });
        }

        public void Enable()
        {
            hitbox.Enable();
        }

        public void Disable()
        {
            hitbox.Disable();
        }
    }
}