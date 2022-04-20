using System;
using System.Collections.Generic;
using System.Linq;

namespace Characters.Enemies.States
{
    public class EnemyStateFactory<TContext>
    {
        private readonly IStateHolder<TContext> _stateHolder;
        private readonly TContext _context;
        private readonly Func<EnemyBaseState<TContext>>[] _stateBuilders;

        public EnemyStateFactory(IStateHolder<TContext> stateHolder, TContext context,
            IEnumerable<Func<EnemyBaseState<TContext>>> stateBuilders)
        {
            _stateHolder = stateHolder;
            _context = context;
            _stateBuilders = stateBuilders.ToArray();
        }

        private EnemyBaseState<TContext> CreateState(int index)
        {
            var state = _stateBuilders[index]();
            state.Init(_stateHolder, _context, this);
            return state;
        }

        public EnemyBaseState<TContext> Idle()
        {
            return CreateState(0);
        }

        public EnemyBaseState<TContext> Pursue()
        {
            return CreateState(1);
        }

        public EnemyBaseState<TContext> Attack()
        {
            return CreateState(2);
        }

        public EnemyBaseState<TContext> Stagger()
        {
            return CreateState(3);
        }
    }
}