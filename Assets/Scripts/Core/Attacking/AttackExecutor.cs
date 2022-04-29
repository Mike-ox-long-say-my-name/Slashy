using System;
using System.Collections;
using Core.Attacking.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Attacking
{
    public abstract class AttackExecutor : IAttackExecutor
    {
        private Coroutine _runningAttack;
        private Action<bool> _attackEnded;

        protected ICoroutineHost Host { get; }
        protected IAttackbox Attackbox { get; }

        public bool IsAttacking => _runningAttack != null;

        protected AttackExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            Guard.NotNull(host);
            Guard.NotNull(attackbox);
            
            Host = host;
            Attackbox = attackbox;
        }

        public virtual void InterruptAttack()
        {
            if (!IsAttacking)
            {
                Debug.LogWarning("Trying to interrupt inactive attack");
                return;
            }

            OnAttackEnded(true);
            Host.Stop(_runningAttack);
            _runningAttack = null;
            _attackEnded = null;
        }

        public void StartAttack(Action<bool> attackEnded)
        {
            if (IsAttacking)
            {
                Debug.LogWarning("Trying to start attack while already attacking");
                return;
            }

            _attackEnded = attackEnded;
            _runningAttack = Host.Start(ExecuteInternal());
        }

        private IEnumerator ExecuteInternal()
        {
            yield return Execute();
            
            OnAttackEnded(false);
            _runningAttack = null;
            _attackEnded = null;
        }

        protected abstract IEnumerator Execute();

        protected virtual void OnAttackEnded(bool interrupted)
        {
            _attackEnded?.Invoke(interrupted);
            _attackEnded = null;
        }
    }
}