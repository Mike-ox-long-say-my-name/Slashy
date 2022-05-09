using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using System;
using Core.Utilities;

namespace Core.Characters
{
    public class Character : ICharacter
    {
        public Team Team { get; set; }

        public event Action<ICharacter, HitInfo> HitReceived;
        public event Action<ICharacter, HitInfo> Staggered;
        public event Action<ICharacter, HitInfo> Dead;
        public event Action<ICharacter> RecoveredFromStagger;
        
        public bool CanDie { get; set; } = true;

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
            _staggerTimer.Timeout += () => RecoveredFromStagger?.Invoke(this);
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
                Staggered?.Invoke(this, info);
            }
            else
            {
                HitReceived?.Invoke(this, info);
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

        protected virtual void Die(HitInfo info)
        {
            if (IsDead)
            {
                return;
            }

            IsDead = true;
            Dead?.Invoke(this, info);
        }

        public void Tick(float deltaTime)
        {
            _staggerTimer.Tick(deltaTime);
        }
    }
}