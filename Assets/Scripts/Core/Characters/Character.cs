using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.Utilities;
using System;

namespace Core.Characters
{
    public class Character : ICharacter
    {
        public event Action<ICharacter, ICharacterResource> OnHealthChanged;
        public event Action<ICharacter, HitInfo> OnHitReceivedExclusive;
        public event Action<IHitReceiver, HitInfo> OnHitReceived;
        public event Action<ICharacter, HitInfo> OnStaggered;
        public event Action<ICharacter, HitInfo> OnDeath;

        public IVelocityMovement VelocityMovement { get; }

        public ICharacterResource Health => _health;
        public ICharacterResource Balance => _balance;

        public DamageStats DamageStats { get; set; }
        public CharacterStats CharacterStats { get; set; }

        private readonly HealthResource _health;
        private readonly BalanceResource _balance;

        public bool IsDead { get; private set; }

        public Character(IVelocityMovement movement, DamageStats damageStats, CharacterStats characterStats)
        {
            Guard.NotNull(movement);

            VelocityMovement = movement;
            DamageStats = damageStats;
            CharacterStats = characterStats;

            _health = new HealthResource(this, CharacterStats.MaxHealth);
            _balance = new BalanceResource(this, CharacterStats.MaxBalance);
        }

        public virtual void ReceiveHit(HitInfo info)
        {
            if (IsDead)
            {
                return;
            }

            var dead = TakeDamage(info);
            var staggered = TakeBalanceDamage(info);

            if (staggered && !dead)
            {
                var source = info.Source;

                var pushTime = info.PushTime;
                var pushForce = info.PushForce;

                if (source.Character != null && pushTime > 0 && pushForce > 0)
                {
                    var sourcePosition = source.Character.VelocityMovement.Movement.Transform.position;
                    var direction = (VelocityMovement.Movement.Transform.position - sourcePosition);
                    direction.y = 0;
                    direction.Normalize();
                    VelocityMovement.Pushable.Push(direction, pushForce, pushTime);
                }

                _balance.Recover(CharacterStats.MaxBalance);
            }

            OnHitReceived?.Invoke(this, info);

            if (dead)
            {
                Die(info);
            }
            else if (staggered)
            {
                OnStaggered?.Invoke(this, info);
            }
            else
            {
                OnHitReceivedExclusive?.Invoke(this, info);
            }
        }

        protected virtual bool TakeBalanceDamage(HitInfo info)
        {
            if (CharacterStats.FreezeBalance)
            {
                return false;
            }

            _balance.Spend(info.BalanceDamage);
            return _balance.IsDepleted;
        }

        protected virtual bool TakeDamage(HitInfo info)
        {
            if (CharacterStats.FreezeHealth)
            {
                return false;
            }

            _health.Spend(info.Damage);
            OnHealthChanged?.Invoke(this, Health);

            return CharacterStats.CanDie && _health.IsDepleted;
        }

        public virtual void Heal(float amount)
        {
            if (CharacterStats.FreezeHealth)
            {
                return;
            }

            _health.Recover(amount);
            OnHealthChanged?.Invoke(this, Health);
        }

        public virtual void Tick(float deltaTime)
        {
            VelocityMovement.Tick(deltaTime);
        }

        protected virtual void Die(HitInfo info)
        {
            if (IsDead)
            {
                return;
            }
            IsDead = true;
            OnDeath?.Invoke(this, info);
        }
    }
}