using System;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.DependencyInjection;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters.Mono
{
    public abstract class MonoBaseCharacter : MonoBehaviour, IMonoCharacter, ICharacterEventDispatcher
    {
        public UnityEvent<ICharacter, ICharacterResource> OnHealthChanged { get; } =
            new UnityEvent<ICharacter, ICharacterResource>();

        public UnityEvent<IHitReceiver, HitInfo> OnHitReceived { get; } = new UnityEvent<IHitReceiver, HitInfo>();

        public UnityEvent<ICharacter, HitInfo> OnHitReceivedExclusive { get; } = new UnityEvent<ICharacter, HitInfo>();

        public UnityEvent<ICharacter, HitInfo> OnStaggered { get; } = new UnityEvent<ICharacter, HitInfo>();

        public UnityEvent<ICharacter, HitInfo> OnDeath { get; } = new UnityEvent<ICharacter, HitInfo>();

        private ICharacter _character;
        
        [AutoResolve]
        ICharacter IMonoCharacter.Resolve()
        {
            if (_character != null)
            {
                return _character;
            }

            _character = CreateCharacter();
            return _character;
        }

        IHitReceiver IMonoWrapper<IHitReceiver>.Resolve()
        {
            return ((IMonoCharacter) this).Resolve();
        }

        protected virtual void Update()
        {
            _character.Tick(Time.deltaTime);
        }

        protected abstract ICharacter CreateCharacter();

        void ICharacterEventDispatcher.OnHealthChanged(ICharacter character, ICharacterResource health)
        {
            OnHealthChanged?.Invoke(character, health);
        }

        void ICharacterEventDispatcher.OnHitReceived(ICharacter character, HitInfo info)
        {
            OnHitReceived?.Invoke(character, info);
        }

        void ICharacterEventDispatcher.OnHitReceivedExclusive(ICharacter character, HitInfo info)
        {
            OnHitReceivedExclusive?.Invoke(character, info);
        }

        void ICharacterEventDispatcher.OnStaggered(ICharacter character, HitInfo info)
        {
            OnStaggered?.Invoke(character, info);
        }

        void ICharacterEventDispatcher.OnDeath(ICharacter character, HitInfo info)
        {
            OnDeath?.Invoke(character, info);
        }
    }
}