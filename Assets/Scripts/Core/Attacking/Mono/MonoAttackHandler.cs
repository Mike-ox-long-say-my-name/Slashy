using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Utilities;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    public abstract class MonoAttackHandler : MonoBehaviour
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

        private void OnHit(IHitbox source, IHurtbox hit)
        {
            hit.ProcessHit(HitInfo);
        }

        public IAttackExecutor Executor
        {
            get
            {
                if (_executor != null)
                {
                    return _executor;
                }

                var monoCharacter = GetComponentInParent<MonoCharacter>();
                _source = !isEnvironmental && monoCharacter != null ? monoCharacter.Character : null;

                var attackBox = GetComponentInChildren<MonoAttackbox>().Attackbox;
                attackBox.OnHit += OnHit;
                _executor = CreateExecutor(this.ToCoroutineHost(), attackBox);

                return _executor;
            }
        }
    }
}