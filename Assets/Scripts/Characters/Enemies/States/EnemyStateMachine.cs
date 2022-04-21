using Characters.Player;
using Core.Characters;
using UnityEngine;

namespace Characters.Enemies.States
{
    public abstract class EnemyStateMachine<T> : MonoBehaviour, IStateHolder<T>
    {
        public EnemyBaseState<T> CurrentState { get; set; }

        [SerializeField] private Character character;
        [SerializeField] private CharacterMovement characterMovement;

        public Character Character => character;
        public CharacterMovement Movement => characterMovement;
        public BasePlayerData PlayerData => PlayerManager.Instance.PlayerData;
        public Vector3 PlayerPosition => PlayerData.Movement.transform.position;

        protected abstract EnemyBaseState<T> StartState();

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
                character.OnHitReceived.AddListener((source, info) => 
                    CurrentState.InterruptState(new CharacterInterruption(CharacterInterruptionType.Hit, source)));
                character.OnStaggered.AddListener(() =>
                    CurrentState.InterruptState(new CharacterInterruption(CharacterInterruptionType.Staggered, null)));
                character.OnDeath.AddListener(() => 
                    CurrentState.InterruptState(new CharacterInterruption(CharacterInterruptionType.Death, null)));
            }

            CurrentState = StartState();
            CurrentState.EnterState();
        }

        protected virtual void Update()
        {
            CurrentState.UpdateState();
        }
    }
}