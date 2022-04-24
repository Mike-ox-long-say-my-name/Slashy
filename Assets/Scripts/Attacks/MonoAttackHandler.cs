using Core.Attacking;
using Core.Characters;
using UnityEngine;

namespace Attacks
{
    [DisallowMultipleComponent]
    public abstract class MonoAttackHandler : MonoBehaviour, IMonoAttackHandler, IMonoHitEventReceiver
    {
        [SerializeField] private bool isEnvironmental;
        [SerializeField] private DamageInfo damageInfo;

        public IAttackExecutor Executor { get; private set; }

        protected HitInfo HitInfo { get; private set; }

        private void CreateHitInfo()
        {
            var character = !isEnvironmental ? GetComponentInParent<IMonoCharacter>()?.Native : null;
            HitInfo = new HitInfo
            {
                DamageInfo = damageInfo,
                Source = new HitSource
                {
                    Character = character,
                    IsEnvironmental = isEnvironmental
                }
            };
        }

        protected abstract IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox);

        protected virtual void Start()
        {
            CreateHitInfo();

            var attackBox = GetComponentInChildren<IMonoAttackbox>()?.Attackbox;
            Executor = CreateExecutor(this.ToCoroutineHost(), attackBox);
        }

        public void OnHit(IHitbox source, IHurtbox hit)
        {
            hit.Dispatch(HitInfo);
        }
    }
}