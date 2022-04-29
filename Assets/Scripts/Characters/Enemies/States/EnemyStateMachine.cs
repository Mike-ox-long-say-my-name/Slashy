using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Player;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Enemies.States
{
    public abstract class EnemyStateMachine<T> : MonoBehaviour, IStateHolder<T>
    {
        public EnemyBaseState<T> CurrentState { get; set; }
        
        public ICharacter Character { get; private set; }
        public ICharacterMovement Movement => Character.Movement;
        public IPushable Pushable => Character.Pushable;
        public IHurtbox Hurtbox { get; private set; }

        public IMonoPlayerInfoProvider MonoPlayerInfo => PlayerManager.Instance.PlayerInfo;
        public IPlayerCharacter Player => MonoPlayerInfo.Player;

        public Vector3 PlayerPosition => Player.Movement.Transform.position;

        protected abstract EnemyBaseState<T> StartState();

        protected virtual void Awake()
        {
            Hurtbox = GetComponentInChildren<MonoHurtbox>().Hurtbox;

            var character = Character = GetComponent<MonoCharacter>().Character;
            character.OnHitReceivedExclusive += (_, info) => CurrentState.OnHitReceived(info);
            character.OnStaggered += (_, info) => CurrentState.OnStaggered(info);
            character.OnDeath += (_, info) => CurrentState.OnDeath(info);

            CurrentState = StartState();
            CurrentState.EnterState();
        }

        protected virtual void Update()
        {
            CurrentState.UpdateState();
        }
    }
}