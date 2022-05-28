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
        private readonly CoroutineContext _runningAttack;
        private Action<AttackResult> _attackEnded;

        private ICoroutineRunner CoroutineRunner { get; }
        
        protected IAttackbox Attackbox { get; }

        public bool IsAttacking { get; private set; }

        private readonly List<IHurtbox> _hits = new List<IHurtbox>();

        public void RegisterHit(IHurtbox hit)
        {
            _hits.Add(hit);
        }

        protected AttackExecutor(ICoroutineRunner coroutineRunner, IAttackbox attackbox)
        {
            Guard.NotNull(coroutineRunner);
            Guard.NotNull(attackbox);
            
            CoroutineRunner = coroutineRunner;
            Attackbox = attackbox;

            Attackbox.Hit += RegisterHit;

            _runningAttack = CoroutineRunner.GetEmptyContext();
        }

        public virtual void InterruptAttack()
        {
            if (!IsAttacking)
            {
                Debug.LogWarning("Trying to interrupt inactive attack");
                return;
            }
            
            _runningAttack.Stop();
            OnAttackEnded(true);
        }

        public void StartAttack(Action<AttackResult> attackEnded)
        {
            if (IsAttacking)
            {
                Debug.LogWarning("Trying to start attack while already attacking");
                return;
            }

            IsAttacking = true;
            _attackEnded = attackEnded;
            _runningAttack.Start(ExecuteInternal());
        }

        private IEnumerator ExecuteInternal()
        {
            yield return Execute();
            
            OnAttackEnded(false);
        }

        protected abstract IEnumerator Execute();

        protected virtual void OnAttackEnded(bool interrupted)
        {
            IsAttacking = false;
            var previousAttackEndedCallback = _attackEnded;
            _attackEnded = null;

            var attackResult = new AttackResult(_hits, !interrupted);
            previousAttackEndedCallback?.Invoke(attackResult);
            
            _hits.Clear();
        }
    }
}