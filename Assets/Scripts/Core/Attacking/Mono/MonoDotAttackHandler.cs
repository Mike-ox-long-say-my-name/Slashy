using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.DependencyInjection;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MonoDotAttackHandler : MonoBehaviour, IMonoDotAttackExecutor, IMonoHitEventReceiver
    {
        [SerializeField] private bool isEnvironmental;
        [SerializeField] private DamageInfo damageInfo;

        private IDotAttackExecutor _executor;
        private HitInfo _hitInfo;

        private void Awake()
        {
            var character = !isEnvironmental ? GetComponentInParent<IMonoCharacter>()?.Resolve() : null;

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
        
        [AutoResolve]
        public IDotAttackExecutor Resolve()
        {
            if (_executor != null)
            {
                return _executor;
            }

            var attackBox = GetComponentInChildren<IMonoDotAttackbox>()?.Resolve();
            _executor = new DotAttackExecutor(attackBox);
            return _executor;
        }
    }
}