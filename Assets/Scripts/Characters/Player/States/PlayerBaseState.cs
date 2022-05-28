using Core;
using Core.Attacking;
using UnityEngine;

namespace Characters.Player.States
{
    public abstract class PlayerBaseState
    {
        protected PlayerStateMachine Context { get; private set; }
        private bool IsValidState { get; set; } = true;

        public void Init(PlayerStateMachine context)
        {
            Context = context;
        }

        protected virtual void ApplyMoveInput()
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
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<PlayerDeathState>(true);
        }

        public virtual void OnHitReceived(HitInfo info)
        {
        }

        public virtual void OnStaggered(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
        }

        public virtual void OnStaggerEnded()
        {
        }

        public virtual void OnInteracted()
        {
            Context.Interactor.TryInteract(InteractionMask.Popup);
        }

        protected virtual void SwitchState<T>(bool ignoreValidness = false) where T : PlayerBaseState, new()
        {
            if (!ignoreValidness && !IsValidState)
            {
                return;
            }
            
            var newState = new T();
            newState.Init(Context);

            IsValidState = false;
            ExitState();
            Context.CurrentState = newState;
            newState.EnterState();
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public virtual void OnWarpStarted(Vector3 target)
        {
            Context.WarpPosition = target;
        }

        public virtual void OnWarpEnded(Vector3 target)
        {
            Context.WarpPosition = target;
        }
    }
}