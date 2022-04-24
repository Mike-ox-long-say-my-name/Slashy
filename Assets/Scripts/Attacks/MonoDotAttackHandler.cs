using Core.Attacking;
using Core.Characters;
using UnityEngine;

namespace Attacks
{
    public class MonoDotAttackHandler : MonoBehaviour, IMonoHitEventReceiver
    {
        [SerializeField] private bool isEnvironmental;
        [SerializeField] private DamageInfo damageInfo;

        public IDotAttackExecutor Executor { get; protected set; }

        private HitInfo _hitInfo;

        private void Start()
        {
            var character = !isEnvironmental ? GetComponentInParent<IMonoCharacter>()?.Native : null;
            var attackBox = GetComponentInChildren<IMonoDotAttackbox>()?.Attackbox;
            Executor = new DotAttackExecutor(attackBox);

            _hitInfo = new HitInfo
            {
                DamageInfo = damageInfo,
                Source = new HitSource
                {
                    IsEnvironmental = isEnvironmental,
                    Character = character
                }
            };
        }

        public void OnHit(IHitbox source, IHurtbox hit)
        {
            hit.Dispatch(_hitInfo);
        }
    }
}