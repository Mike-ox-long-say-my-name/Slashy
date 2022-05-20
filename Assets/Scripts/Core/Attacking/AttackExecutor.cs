using System;
using System.Collections;
using System.Collections.Generic;
using Core.Attacking.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Attacking
{
    public abstract class AttackExecutor : IAttackExecutor
    {
        private Coroutine _runningAttack;
        private Action<AttackResult> _attackEnded;

        protected ICoroutineHost Host { get; }
        protected IAttackbox Attackbox { get; }

        public bool IsAttacking => _runningAttack != null;

        private readonly List<IHurtbox> _hits = new List<IHurtbox>();

        public void RegisterHit(IHurtbox hit)
        {
            _hits.Add(hit);
        }

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
            
            Host.Stop(_runningAttack);
            OnAttackEnded(true);
        }

        public void StartAttack(Action<AttackResult> attackEnded)
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
        }

        protected abstract IEnumerator Execute();

        protected virtual void OnAttackEnded(bool interrupted)
        {
            _runningAttack = null;
            var temp = _attackEnded;
            _attackEnded = null;
            temp?.Invoke(new AttackResult(_hits, !interrupted));
            _hits.Clear();
        }
    }
}