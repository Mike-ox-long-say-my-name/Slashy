using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters
{
    [RequireComponent(typeof(Pushable))]
    public class Character : HittableEntity
    {
        [SerializeField] private UnityEvent<ICharacterResource> onHealthChanged;
        public UnityEvent<ICharacterResource> OnHealthChanged => onHealthChanged;

        [SerializeField] private UnityEvent onStaggered;
        public UnityEvent OnStaggered => onStaggered;
        
        [SerializeField] private UnityEvent onDeath;
        public UnityEvent OnDeath => onDeath;

        [SerializeField, Min(0)] private float maxHealth;
        [SerializeField, Min(0)] private float maxBalance;
        [SerializeField] private bool freezeHealth = false;
        [SerializeField] private bool canDie = true;

        private Pushable _pushable;
        
        private BalanceResource _balanceResource;
        private HealthResource _healthResource;
        public ICharacterResource Health => _healthResource;

        protected virtual void Awake()
        {
            _pushable = GetComponent<Pushable>();
            _healthResource = new HealthResource(this, maxHealth);
            _balanceResource = new BalanceResource(this, maxBalance);
        }

        public override void ReceiveHit(HitInfo info)
        {
            base.ReceiveHit(info);
            TakeDamage(info);
            if (!_healthResource.IsDepleted)
            {
                TakeBalanceDamage(info);
            }
        }

        public virtual void TakeBalanceDamage(HitInfo info)
        {
            _balanceResource.Spend(info.DamageInfo.balanceDamage);
            if (!_balanceResource.IsDepleted)
            {
                return;
            }

            var source = info.HitSource;
            if (source != null && info.DamageInfo.pushStrength > 0)
            {
                var direction = (transform.position - source.Transform.position).normalized;
                _pushable.Push(direction, info.DamageInfo.pushStrength);
            }

            // TODO: добавить зависимость длительности стаггера от DamageInfo
            OnStaggered?.Invoke();
            _balanceResource.Recover(maxBalance);
        }

        public virtual void TakeDamage(HitInfo info)
        {
            if (freezeHealth)
            {
                return;
            }

            _healthResource.Spend(info.DamageInfo.damage);
            OnHealthChanged?.Invoke(Health);

            if (canDie && _healthResource.IsDepleted)
            {
                Die();
            }
        }

        public virtual void Heal(float amount)
        {
            if (freezeHealth)
            {
                return;
            }

            _healthResource.Recover(amount);
            OnHealthChanged?.Invoke(Health);
        }

        protected virtual void Die()
        {
            OnDeath?.Invoke();
        }
    }
}