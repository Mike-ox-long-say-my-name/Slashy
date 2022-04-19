namespace Characters.Enemies.States
{
    public abstract class EnemyBaseState
    {
        protected EnemyStateMachine Context { get; }
        protected EnemyStateFactory Factory { get; }

        protected EnemyBaseState(EnemyStateMachine context, EnemyStateFactory factory)
        {
            Context = context;
            Factory = factory;
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

        public virtual void OnStaggered()
        {
            SwitchState(Factory.Stagger());
        }

        protected virtual void SwitchState(EnemyBaseState newState)
        {
            ExitState();
            newState.EnterState();
            Context.CurrentState = newState;
        }
    }
}