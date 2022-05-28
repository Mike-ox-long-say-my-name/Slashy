using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Modules;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Enemies.States
{
    [RequireComponent(typeof(MixinVelocityMovement))]
    [RequireComponent(typeof(MixinCharacter))]
    [RequireComponent(typeof(DestroyHelper))]
    public abstract class EnemyStateMachine<T> : MonoBehaviour, IStateHolder<T> where T : class, IStateHolder<T>
    {
        public EnemyBaseState<T> CurrentState { get; set; }
        public ICharacter Character { get; private set; }
        public IVelocityMovement VelocityMovement { get; private set; }
        public IBaseMovement BaseMovement => VelocityMovement.BaseMovement;
        public IHurtbox Hurtbox { get; private set; }
        public DestroyHelper DestroyHelper { get; private set; }
        public AttackExecutorHelper AttackExecutorHelper { get; private set; }
        private IAggroListener _aggroListener;
        public IPlayer Player => _lazyPlayer.Value;
        private LazyPlayer _lazyPlayer;

        public Vector3 PlayerPosition => Player.Transform.position;

        public void Aggro() => _aggroListener.IncreaseAggroCounter();

        public void Deaggro() => _aggroListener.DecreaseAggroCounter();

        protected abstract EnemyBaseState<T> StartState();

        protected virtual void Awake()
        {
            Construct();
            SubscribeToCharacterEvents();
        }

        private void Construct()
        {
            VelocityMovement = GetComponent<MixinVelocityMovement>().VelocityMovement;
            Hurtbox = GetComponentInChildren<MonoHurtbox>().Hurtbox;
            DestroyHelper = GetComponent<DestroyHelper>();

            AttackExecutorHelper = new AttackExecutorHelper(
                GetComponentsInChildren<MonoAbstractAttackExecutor>());
            _lazyPlayer = Container.Get<IPlayerFactory>().GetLazyPlayer();
            _aggroListener = Container.Get<IAggroListener>();
        }

        private void Start()
        {
            CurrentState = StartState();
            CurrentState.EnterState();
        }

        private void SubscribeToCharacterEvents()
        {
            var character = Character = GetComponent<MixinCharacter>().Character;
            character.HitReceived += info => CurrentState.OnHitReceived(info);
            character.Staggered += info => CurrentState.OnStaggered(info);
            character.Dead += info => CurrentState.OnDeath(info);
            character.RecoveredFromStagger += () => CurrentState.OnStaggerEnded();
        }

        protected virtual void Update()
        {
            CurrentState.UpdateState();
        }
    }
}