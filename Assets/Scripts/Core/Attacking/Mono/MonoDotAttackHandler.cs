using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MonoDotAttackHandler : MonoBehaviour
    {
        [SerializeField] private bool isEnvironmental;
        [SerializeField] private DamageInfo damageInfo;

        private ICharacter _source;
        private IDotAttackExecutor _executor;

        private void Awake()
        {
            var monoCharacter = GetComponentInChildren<MonoCharacter>();
            _source = !isEnvironmental && monoCharacter != null ? monoCharacter.Character : null;
        }

        private void OnHit(IHitbox source, IHurtbox hit)
        {
            var hitInfo = new HitInfo
            {
                DamageInfo = damageInfo,
                Source = new HitSource
                {
                    IsEnvironmental = isEnvironmental,
                    Character = _source
                }
            };
            hit.ProcessHit(hitInfo);
        }

        public IDotAttackExecutor Executor
        {
            get
            {
                if (_executor != null)
                {
                    return _executor;
                }

                var attackBox = GetComponentInChildren<MonoDotAttackbox>().Attackbox;
                attackBox.OnHit += OnHit;
                _executor = new DotAttackExecutor(attackBox);
                return _executor;
            }
        }
    }
}