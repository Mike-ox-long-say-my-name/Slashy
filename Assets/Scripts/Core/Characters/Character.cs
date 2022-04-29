using System;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Characters
{
    public class Character : ICharacter
    {
        public GameObject Object { get; }
        
        public event Action<ICharacter, ICharacterResource> OnHealthChanged;
        public event Action<ICharacter, HitInfo> OnHitReceivedExclusive;
        public event Action<IHitReceiver, HitInfo> OnHitReceived;
        public event Action<ICharacter, HitInfo> OnStaggered;
        public event Action<ICharacter, HitInfo> OnDeath;

        public ICharacterMovement Movement { get; }
        public IPushable Pushable { get; }

        public ICharacterResource Health => _health;
        public ICharacterResource Balance => _balance;

        private readonly ICharacterStats _stats;

        private readonly HealthResource _health;
        private readonly BalanceResource _balance;

        public Character(ICharacterMovement movement, IPushable pushable, ICharacterStats stats)
        {
            Guard.NotNull(movement);
            Guard.NotNull(pushable);
            Guard.NotNull(stats);

            Object = movement.Controller.gameObject;

            Movement = movement;
            Pushable = pushable;

            _stats = stats;

            _health = new HealthResource(this, _stats.MaxHealth);
            _balance = new BalanceResource(this, _stats.MaxBalance);
        }

        public virtual void ReceiveHit(HitInfo info)
        {
            var dead = TakeDamage(info);
            var staggered = TakeBalanceDamage(info);

            if (staggered && !dead)
            {
                var source = info.Source;
                if (source.Character != null && info.DamageInfo.pushStrength > 0)
                {
                    var sourcePosition = source.Character.Movement.Transform.position;
                    var direction = (Movement.Transform.position - sourcePosition);
                    direction.y = 0;
                    direction.Normalize();
                    Pushable.Push(direction, info.DamageInfo.pushStrength);
                }

                _balance.Recover(_stats.MaxBalance);
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
            _balance.Spend(info.DamageInfo.balanceDamage);
            return _balance.IsDepleted;
        }

        protected virtual bool TakeDamage(HitInfo info)
        {
            if (_stats.FreezeHealth)
            {
                return false;
            }

            _health.Spend(info.DamageInfo.damage);
            OnHealthChanged?.Invoke(this, Health);

            return _stats.CanDie && _health.IsDepleted;
        }

        public virtual void Heal(float amount)
        {
            if (_stats.FreezeHealth)
            {
                return;
            }

            _health.Recover(amount);
            OnHealthChanged?.Invoke(this, Health);
        }

        public virtual void Tick(float deltaTime)
        {
            Movement.Tick(deltaTime);
            Pushable.Tick(deltaTime);
        }

        protected virtual void Die(HitInfo info)
        {
            OnDeath?.Invoke(this, info);
        }
    }
}