using Core.Attacking;

namespace Characters.Enemies.States
{
    public abstract class EnemyBaseState<TContext>
    {
        private IStateHolder<TContext> _stateHolder;

        protected bool IsValidState { get; private set; } = true;
        protected TContext Context { get; private set; }

        public void Init(IStateHolder<TContext> stateHolder, TContext context)
        {
            _stateHolder = stateHolder;
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

        protected virtual void SwitchState<TState>() where TState : EnemyBaseState<TContext>, new()
        {
            if (!IsValidState)
            {
                return;
            }

            var newState = new TState();
            newState.Init(_stateHolder, Context);

            ExitState();
            IsValidState = false;
            _stateHolder.CurrentState = newState;

            newState.EnterState();
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}