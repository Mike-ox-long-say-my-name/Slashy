using Core.Attacking;

namespace Characters.Enemies.States
{
    public abstract class EnemyBaseState<TContext> where TContext : class, IStateHolder<TContext>
    {
        private bool IsValidState { get; set; } = true;
        protected TContext Context { get; private set; }

        public void Init(TContext context)
        {
            Context = context;
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

        public virtual void OnStaggerEnded()
        {
        }

        protected virtual void SwitchState<TState>(bool ignoreValidness = false) where TState : EnemyBaseState<TContext>, new()
        {
            if (!ignoreValidness && !IsValidState)
            {
                return;
            }

            var newState = new TState();
            newState.Init(Context);

            ExitState();
            IsValidState = false;
            Context.CurrentState = newState;

            newState.EnterState();
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}