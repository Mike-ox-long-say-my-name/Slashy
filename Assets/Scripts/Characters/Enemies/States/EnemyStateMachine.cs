using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.Player;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Enemies.States
{
    public abstract class EnemyStateMachine<T> : MonoBehaviour, IStateHolder<T>
    {
        public EnemyBaseState<T> CurrentState { get; set; }

        public IMonoCharacter MonoCharacter { get; private set; }
        public ICharacter Character { get; private set; }
        public ICharacterMovement Movement => Character.Movement;
        public IPushable Pushable => Character.Pushable;
        public IHurtbox Hurtbox { get; private set; }

        public IMonoPlayerInfoProvider MonoPlayerInfo => PlayerManager.Instance.PlayerInfo;
        public IMonoPlayerCharacter MonoPlayer => MonoPlayerInfo.Player;
        public IPlayerCharacter Player => MonoPlayer.Resolve();

        public Vector3 PlayerPosition => Player.Movement.Transform.position;

        protected abstract EnemyBaseState<T> StartState();

        protected virtual void Awake()
        {
            Hurtbox = GetComponentInChildren<IMonoHurtbox>()?.Resolve();

            var character = MonoCharacter = GetComponent<IMonoCharacter>();
            character.OnHitReceivedExclusive.AddListener((_, info) => CurrentState.OnHitReceived(info));
            character.OnStaggered.AddListener((_, info) => CurrentState.OnStaggered(info));
            character.OnDeath.AddListener((_, info) => CurrentState.OnDeath(info));

            Character = character.Resolve();

            CurrentState = StartState();
            CurrentState.EnterState();
        }

        protected virtual void Update()
        {
            CurrentState.UpdateState();
        }
    }
}