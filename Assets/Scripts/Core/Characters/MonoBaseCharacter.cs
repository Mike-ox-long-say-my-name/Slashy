using Core.Attacking;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters
{
    public abstract class MonoBaseCharacter : MonoBehaviour, IMonoCharacter, ICharacterEventDispatcher
    {
        [SerializeField] private UnityEvent<ICharacter, ICharacterResource> onHealthChanged;
        public UnityEvent<ICharacter, ICharacterResource> OnHealthChanged => onHealthChanged;

        [SerializeField] private UnityEvent<IHitReceiver, HitInfo> onHitReceived;
        public UnityEvent<IHitReceiver, HitInfo> OnHitReceived => onHitReceived;

        [SerializeField] private UnityEvent<ICharacter, HitInfo> onHitReceivedExclusive;
        public UnityEvent<ICharacter, HitInfo> OnHitReceivedExclusive => onHitReceivedExclusive;

        [SerializeField] private UnityEvent<ICharacter, HitInfo> onStaggered;
        public UnityEvent<ICharacter, HitInfo> OnStaggered => onStaggered;

        [SerializeField] private UnityEvent<ICharacter, HitInfo> onDeath;
        public UnityEvent<ICharacter, HitInfo> OnDeath => onDeath;

        public ICharacter Native { get; protected set; }
        IHitReceiver IMonoWrapper<IHitReceiver>.Native => Native;

        protected virtual void Awake()
        {
            Native = CreateCharacter();
        }

        protected virtual void Update()
        {
            Native.Tick(Time.deltaTime);
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