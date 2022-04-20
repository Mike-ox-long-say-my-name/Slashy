using System;
using Core.Characters;

namespace Characters.Enemies.States
{
    public abstract class EnemyBaseState<TContext>
    {
        private IStateHolder<TContext> _stateHolder;

        protected TContext Context { get; private set; }
        protected EnemyStateFactory<TContext> Factory { get; private set; }
        
        public void Init(IStateHolder<TContext> stateHolder, TContext context, EnemyStateFactory<TContext> factory)
        {
            _stateHolder = stateHolder;
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

        public virtual void InterruptState(CharacterInterruption interruption)
        {
            switch (interruption)
            {
                case CharacterInterruption.Staggered:
                    SwitchState(Factory.Stagger());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interruption), interruption, 
                        $"Invalid interruption for state {this}");
            }
        }

        protected virtual void SwitchState(EnemyBaseState<TContext> newState)
        {
            ExitState();
            _stateHolder.CurrentState = newState;
            newState.EnterState();
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}