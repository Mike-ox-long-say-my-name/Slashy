using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Modules;
using Core.Player;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Enemies.States
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MixinPushable))]
    [RequireComponent(typeof(MixinVelocityMovement))]
    [RequireComponent(typeof(MixinCharacter))]
    [RequireComponent(typeof(MixinDestroyable))]
    public abstract class EnemyStateMachine<T> : MonoBehaviour, IStateHolder<T>
    {
        public EnemyBaseState<T> CurrentState { get; set; }
        
        public Animator Animator { get; private set; }
        public ICharacter Character { get; private set; }
        public IVelocityMovement VelocityMovement { get; private set; }
        public IBaseMovement BaseMovement => VelocityMovement.BaseMovement;
        public IPushable Pushable { get; private set; }
        public IHurtbox Hurtbox { get; private set; }
        public MixinDestroyable Destroyable { get; private set; }

        public IPlayer PlayerInfo => PlayerManager.Instance.PlayerInfo;
        public IPlayerCharacter Player => PlayerInfo.Player;

        public Vector3 PlayerPosition => PlayerInfo.VelocityMovement.BaseMovement.Transform.position;

        public HitInfo LastHit { get; set; }

        protected abstract EnemyBaseState<T> StartState();

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            Pushable = GetComponent<MixinPushable>().Pushable;
            VelocityMovement = GetComponent<MixinVelocityMovement>().VelocityMovement;
            Hurtbox = GetComponentInChildren<MonoHurtbox>().Hurtbox;
            Destroyable = GetComponent<MixinDestroyable>();

            var character = Character = GetComponent<MixinCharacter>().Character;
            character.HitReceived += (_, info) => CurrentState.OnHitReceived(info);
            character.Staggered += (_, info) => CurrentState.OnStaggered(info);
            character.Dead += (_, info) => CurrentState.OnDeath(info);
            character.RecoveredFromStagger += _ => CurrentState.OnStaggerEnded();

            CurrentState = StartState();
            CurrentState.EnterState();
        }

        protected virtual void Update()
        {
            CurrentState.UpdateState();
        }
    }
}