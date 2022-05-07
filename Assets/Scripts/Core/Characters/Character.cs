using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using System;

namespace Core.Characters
{
    public class Character : ICharacter
    {
        public event Action<ICharacter, HitInfo> HitReceived;
        public event Action<ICharacter, HitInfo> Staggered;
        public event Action<ICharacter, HitInfo> Dead;

        public bool FreezeHealth { get; set; } = false;
        public bool FreezeBalance { get; set; } = false;
        public bool CanDie { get; set; } = true;

        public IResource Health { get; }
        public IResource Balance { get; }

        public bool IsDead { get; private set; }

        public Character(IResource health, IResource balance, IHitReceiver hitReceiver)
        {
            Health = health;
            Balance = balance;

            hitReceiver.HitReceived += ProcessHit;
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
                Staggered?.Invoke(this, info);
                Balance.Recover(Balance.MaxValue);
            }
            else
            {
                HitReceived?.Invoke(this, info);
            }
        }

        private bool TakeBalanceDamage(HitInfo info)
        {
            if (FreezeBalance)
            {
                return false;
            }

            Balance.Spend(info.BalanceDamage);
            return Balance.IsDepleted();
        }

        private bool TakeDamage(HitInfo info)
        {
            if (FreezeHealth)
            {
                return false;
            }

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
    }
}