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
        public IVelocityMovement VelocityMovement => Character.VelocityMovement;
        public IMovement Movement => VelocityMovement.BaseMovement;
        public IPushable Pushable => VelocityMovement.Pushable;
        public IHurtbox Hurtbox { get; private set; }

        public IPlayer PlayerInfo => PlayerManager.Instance.PlayerInfo;
        public IPlayerCharacter Player => PlayerInfo.Player;

        public Vector3 PlayerPosition => Player.PlayerMovement.BaseMovement.Transform.position;

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

    public static class VectorExtensions
    {
        public static Vector3 WithZeroY(this Vector3 vector)
        {
            vector.y = 0;
            return vector;
        }
    }
}