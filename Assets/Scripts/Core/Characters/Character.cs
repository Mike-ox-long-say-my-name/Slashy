using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using System;
using Core.Utilities;

namespace Core.Characters
{
    public sealed class Character : ICharacter
    {
        public Team Team { get; set; }

        public event Action<HitInfo> HitReceived;
        public event Action<HitInfo> Staggered;
        public event Action<HitInfo> Died;
        public event Action RecoveredFromStagger;
        
        public bool CanDie { get; set; } = true;

        public void Kill()
        {
            Die(null);
        }

        public IResource Health { get; }
        public IResource Balance { get; }
        public IHitReceiver HitReceiver { get; }

        public bool IsDead { get; private set; }

        private readonly Timer _staggerTimer = new Timer();

        public Character(IResource health, IResource balance, IHitReceiver hitReceiver)
        {
            Guard.NotNull(health);
            Guard.NotNull(balance);
            Guard.NotNull(hitReceiver);

            Health = health;
            Balance = balance;
            HitReceiver = hitReceiver;

            hitReceiver.HitReceived += ProcessHit;
            _staggerTimer.Timeout += () => RecoveredFromStagger?.Invoke();
        }

        ~Character()
        {
            HitReceiver.HitReceived -= ProcessHit;
        }

        private void ProcessHit(IHitReceiver receiver, HitInfo info)
        {
            if (IsDead)
            {
                return;
            }

            var dead = TakeDamage(info);
            var staggered = TakeBalanceDamage(info);
            
            if (dead)
            {
                Die(info);
            }
            else if (staggered)
            {
                Balance.Recover(Balance.MaxValue);
                _staggerTimer.Start(info.StaggerTime);
                Staggered?.Invoke(info);
            }
            else
            {
                HitReceived?.Invoke(info);
            }
        }

        private bool TakeBalanceDamage(HitInfo info)
        {
            Balance.Spend(info.BalanceDamage);
            return Balance.IsDepleted();
        }

        private bool TakeDamage(HitInfo info)
        {
            Health.Spend(info.Damage);
            return CanDie && Health.IsDepleted();
        }

        private void Die(HitInfo info)
        {
            if (IsDead)
            {
                return;
            }

            IsDead = true;
            Died?.Invoke(info);
        }

        public void Tick(float deltaTime)
        {
            _staggerTimer.Tick(deltaTime);
        }
    }
}