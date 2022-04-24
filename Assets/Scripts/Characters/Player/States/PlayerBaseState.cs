using Core.Attacking;
using UnityEngine;

namespace Characters.Player.States
{
    public abstract class PlayerBaseState
    {
        protected PlayerStateMachine Context { get; }
        protected PlayerStateFactory Factory { get; }

        protected PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory)
        {
            Context = context;
            Factory = factory;
        }

        protected virtual void HandleControl()
        {
            var input = Context.Input.MoveInput;
            var move = new Vector3(input.x, 0, input.y);
            Context.Movement.Move(move.normalized);
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
        }

        public virtual void OnHitReceived(HitInfo info)
        {
        }

        public virtual void OnStaggered(HitInfo info)
        {
        }

        protected virtual void SwitchState(PlayerBaseState newState)
        {
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