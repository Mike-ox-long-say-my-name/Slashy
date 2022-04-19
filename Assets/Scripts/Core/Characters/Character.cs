using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters
{
    public class Character : HittableEntity
    {
        [SerializeField] private UnityEvent<ICharacterResource> onHealthChanged;

        public UnityEvent<ICharacterResource> OnHealthChanged => onHealthChanged;

        [SerializeField, Min(0)] private float maxHealth;
        [SerializeField] private bool freezeHealth = false;
        [SerializeField] private bool canDie = true;

        private HealthResource _healthResource;
        public ICharacterResource Health => _healthResource;

        protected virtual void Awake()
        {
            _healthResource = new HealthResource(this, maxHealth);
        }

        public override void ReceiveHit(HitInfo info)
        {
            base.ReceiveHit(info);
            TakeDamage(info);
        }

        public virtual void TakeDamage(HitInfo info)
        {
            if (freezeHealth)
            {
                return;
            }

            _healthResource.Spend(info.DamageInfo.Damage);
            OnHealthChanged?.Invoke(Health);

            if (canDie && _healthResource.IsDepleted)
            {
                OnDeath();
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

        protected virtual void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}