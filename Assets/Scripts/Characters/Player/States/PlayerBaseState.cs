using System;
using Core.Attacking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Characters.Player.States
{
    public abstract class PlayerBaseState
    {
        protected PlayerStateMachine Context { get; private set; }

        public void Construct(PlayerStateMachine context)
        {
            Context = context;
        }

        protected virtual void HandleControl()
        {
            var input = Context.Input.MoveInput;
            var move = new Vector3(input.x, 0, input.y);
            Context.VelocityMovement.Move(move.normalized);
        }

        public virtual void EnterState()
        {
        }

        public virtual void UpdateState()
        {
        }

        public virtual void ExitState()
        {
        }

        public virtual void OnDeath(HitInfo info)
        {
            SwitchState<PlayerDeathState>();
        }

        public virtual void OnHitReceived(HitInfo info)
        {
            Context.LastHitInfo = info;
        }

        public virtual void OnStaggered(HitInfo info)
        {
        }

        protected virtual void SwitchState<T>() where T : PlayerBaseState, new()
        {
            var newState = new T();
            newState.Construct(Context);

            ExitState();
            Context.CurrentState = newState;
            newState.EnterState();
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}