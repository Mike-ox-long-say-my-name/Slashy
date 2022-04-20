using System;
using System.Linq;
using Characters.Enemies.States;

namespace Characters.Enemies
{
    public class EnemyStateFactoryBuilder<TContext> where TContext : class
    {
        private const int StateCount = 4;

        private readonly Func<EnemyBaseState<TContext>>[] _stateBuilders = new Func<EnemyBaseState<TContext>>[StateCount];

        public EnemyStateFactoryBuilder<TContext> Idle<T>() where T : EnemyBaseState<TContext>, new()
        {
            _stateBuilders[0] = () => new T();
            return this;
        }

        public EnemyStateFactoryBuilder<TContext> Pursue<T>() where T : EnemyBaseState<TContext>, new()
        {
            _stateBuilders[1] = () => new T();
            return this;
        }

        public EnemyStateFactoryBuilder<TContext> Attack<T>() where T : EnemyBaseState<TContext>, new()
        {
            _stateBuilders[2] = () => new T();
            return this;
        }

        public EnemyStateFactoryBuilder<TContext> Stagger<T>() where T : EnemyBaseState<TContext>, new()
        {
            _stateBuilders[3] = () => new T();
            return this;
        }

        public EnemyStateFactory<TContext> Build(IStateHolder<TContext> stateHolder, TContext context)
        {
            if (_stateBuilders.Any(stateBuilder => stateBuilder == null))
            {
                throw new InvalidOperationException("Enemy State Factory is not fully initialized");
            }

            return new EnemyStateFactory<TContext>(stateHolder, context, _stateBuilders);
        }
    }
}