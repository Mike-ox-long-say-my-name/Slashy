using System.Collections.Generic;
using Core.Attacking.Mono;
using System.Linq;
using Core.Attacking.Interfaces;

namespace Core.Attacking
{
    public class AttackExecutorHelper
    {
        private readonly MonoAbstractAttackExecutor[] _executors;

        public AttackExecutorHelper(IEnumerable<MonoAbstractAttackExecutor> executors)
        {
            _executors = executors.ToArray();
        }

        private IEnumerable<IAttackExecutor> GetRunning()
        {
            return _executors
                .Where(executor => executor != null)
                .Select(executor => executor.GetExecutor())
                .Where(executor => executor.IsAttacking);
        }

        public bool IsAllIdle()
        {
            return !IsAnyRunning();
        }

        private bool IsAnyRunning()
        {
            return GetRunning().Any();
        }

        public void InterruptAllRunning()
        {
            foreach (var executor in GetRunning())
            {
                executor.InterruptAttack();
            }
        }
    }
}