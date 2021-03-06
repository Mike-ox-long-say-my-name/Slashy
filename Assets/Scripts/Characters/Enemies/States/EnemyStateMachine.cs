using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters;
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
        public MixinAggro AggroModule { get; private set; }
        public IPlayer Player => _lazyPlayer.Value;
        private LazyPlayer _lazyPlayer;
        private bool _bufferAggroed;

        public Vector3 PlayerPosition => Player.Transform.position;

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
            AggroModule = GetComponent<MixinAggro>();
            AggroModule.Aggroed += () =>
            {
                if (CurrentState != null)
                {
                    CurrentState.OnAggroed();
                }
                else
                {
                    _bufferAggroed = true;
                }
            };

            AttackExecutorHelper = new AttackExecutorHelper(
                GetComponentsInChildren<MonoAbstractAttackExecutor>());
            _lazyPlayer = Container.Get<IPlayerFactory>().GetLazyPlayer();
        }

        private void Start()
        {
            CurrentState = StartState();
            CurrentState.EnterState();
            if (_bufferAggroed)
            {
                CurrentState.OnAggroed();
                _bufferAggroed = false;
            }
        }

        private void SubscribeToCharacterEvents()
        {
            var character = Character = GetComponent<MixinCharacter>().Character;
            character.HitReceived += info => CurrentState.OnHitReceived(info);
            character.Staggered += info => CurrentState.OnStaggered(info);
            character.Died += info =>
            {
                AggroModule.Deaggro();
                CurrentState.OnDeath(info);
            };
            character.RecoveredFromStagger += () => CurrentState.OnStaggerEnded();
        }

        protected virtual void Update()
        {
            CurrentState.UpdateState();
        }
    }
}