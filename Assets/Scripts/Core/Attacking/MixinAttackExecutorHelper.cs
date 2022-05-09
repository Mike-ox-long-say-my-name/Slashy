using System.Collections.Generic;
using Core.Attacking.Mono;
using System.Linq;
using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Attacking
{
    public class MixinAttackExecutorHelper : MonoBehaviour
    {
        private MonoAbstractAttackExecutor[] _executors;

        private void Awake()
        {
            _executors = GetComponentsInChildren<MonoAbstractAttackExecutor>();
        }

        public IEnumerable<IAttackExecutor> GetRunning()
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

        public bool IsAnyRunning()
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