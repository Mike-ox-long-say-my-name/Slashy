namespace Characters.Player.States
{
    public abstract class PlayerBaseState
    {
        protected PlayerStateMachine Context { get; }
        protected PlayerStateFactory Factory { get; }

        protected bool IsRootState { get; set; }
        public PlayerBaseState SuperState { get; private set; }
        public PlayerBaseState SubState { get; private set; }

        protected PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory)
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

        public virtual void UpdateStates()
        {
            UpdateState();
            SubState?.UpdateStates();
        }

        public virtual void ExitStates()
        {
            ExitState();
            SubState?.ExitStates();
        }

        public virtual void OnStaggered()
        {
            if (IsRootState)
            {
                SubState?.OnStaggered();
                SwitchSubState(Factory.Stagger());
            }
            else
            {
                SwitchState(Factory.Stagger());
            }
        }

        protected virtual void SwitchSubState(PlayerBaseState newSubState)
        {
            SubState?.ExitState();
            SetSubState(newSubState);
            SubState.EnterState();
        }

        protected virtual void SwitchState(PlayerBaseState newState)
        {
            ExitState();
            newState.EnterState();

            if (IsRootState)
            {
                Context.CurrentState = newState;
            }
            else
            {
                SuperState?.SetSubState(newState);
            }
        }

        protected void SetSubState(PlayerBaseState newSubState)
        {
            SubState = newSubState;
            newSubState.SetSuperState(this);
        }

        protected void SetSuperState(PlayerBaseState newSuperState)
        {
            SuperState = newSuperState;
        }
    }
}