using Characters.Player;
using Core.Attacking;
using Core.Characters;
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

        public IPlayer Player => PlayerManager.Instance.Player;
        public Vector3 PlayerPosition => Player.Character.Movement.Transform.position;

        protected abstract EnemyBaseState<T> StartState();

        protected virtual void Start()
        {
            Hurtbox = GetComponent<IMonoHurtbox>()?.Native;
            var character = GetComponent<IMonoCharacter>();

            character.OnHitReceivedExclusive.AddListener((_, info) => CurrentState.OnHitReceived(info));
            character.OnStaggered.AddListener((_, info) => CurrentState.OnStaggered(info));
            character.OnDeath.AddListener((_, info) => CurrentState.OnDeath(info));

            Character = character.Native;

            CurrentState = StartState();
            CurrentState.EnterState();
        }

        protected virtual void Update()
        {
            CurrentState.UpdateState();
        }
    }
}