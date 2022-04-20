using Core.Characters;
using System.Collections;
using Characters.Player;
using UnityEngine;

namespace Characters.Enemies.States
{
    public abstract class EnemyStateMachine<T> : MonoBehaviour, IStateHolder<T>
    {
        protected EnemyStateFactory<T> EnemyStateFactory { get; set; }
        public EnemyBaseState<T> CurrentState { get; set; }

        [SerializeField] private Character character;
        [SerializeField] private CharacterMovement characterMovement;

        public Character Character => character;
        public CharacterMovement Movement => characterMovement;
        public BasePlayerData PlayerData => PlayerManager.Instance.PlayerData;
        public Vector3 PlayerPosition => PlayerData.Movement.transform.position;

        protected abstract EnemyStateFactory<T> CreateFactory();

        protected virtual void Awake()
        {
            if (Character == null)
            {
                Debug.LogWarning("Character is not assigned", this);
                enabled = false;
            }
            if (characterMovement == null)
            {
                Debug.LogWarning("Character Movement is not assigned", this);
                enabled = false;
            }
            else
            {
                character.OnStaggered.AddListener(() => CurrentState.InterruptState(CharacterInterruption.Staggered));
            }
        }

        protected virtual IEnumerator Start()
        {
            characterMovement.MoveRaw(Vector3.down);

            EnemyStateFactory = CreateFactory();
            CurrentState = EnemyStateFactory.Idle();
            CurrentState.EnterState();
            CurrentState.UpdateState();

            yield break;
        }

        protected virtual void Update()
        {
            CurrentState.UpdateState();
        }
    }
}