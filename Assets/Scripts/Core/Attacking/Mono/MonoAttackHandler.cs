using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.DependencyInjection;
using Core.Utilities;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    public abstract class MonoAttackHandler : MonoBehaviour, IMonoAttackExecutor, IMonoHitEventReceiver
    {
        [SerializeField] private bool isEnvironmental;
        [SerializeField] private DamageInfo damageInfo;

        private IAttackExecutor _executor;
        private ICharacter _source;

        protected HitInfo HitInfo => new HitInfo
        {
            DamageInfo = damageInfo,
            Source = new HitSource
            {
                Character = _source,
                IsEnvironmental = isEnvironmental
            }
        };

        protected abstract IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox);

        public void OnHit(IHitbox source, IHurtbox hit)
        {
            hit.Dispatch(HitInfo);
        }

        [AutoResolve]
        public IAttackExecutor Resolve()
        {
            if (_executor != null)
            {
                return _executor;
            }

            _source = !isEnvironmental ? GetComponentInParent<IMonoCharacter>()?.Resolve() : null;

            var attackBox = GetComponentInChildren<IMonoAttackbox>()?.Resolve();
            _executor = CreateExecutor(this.ToCoroutineHost(), attackBox);

            return _executor;
        }
    }
}