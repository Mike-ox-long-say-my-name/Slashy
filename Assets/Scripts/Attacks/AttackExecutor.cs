using System.Collections;
using Core.Attacking;
using Core.Characters;
using UnityEngine;

namespace Attacks
{
    public abstract class AttackExecutor : IAttackExecutor
    {
        private Coroutine _runningAttack;
        private IAttackEndEventReceiver _endReceiver;

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
            _endReceiver = null;
        }

        public void StartAttack(IAttackEndEventReceiver endReceiver)
        {
            if (IsAttacking)
            {
                Debug.LogWarning("Trying to start attack while already attacking");
                return;
            }

            _endReceiver = endReceiver;
            _runningAttack = Host.Start(ExecuteInternal());
        }


        public virtual bool InterceptHit(IHurtbox hit)
        {
            return true;
        }

        private IEnumerator ExecuteInternal()
        {
            yield return Execute();
            
            OnAttackEnded(false);
            _runningAttack = null;
            _endReceiver = null;
        }

        protected abstract IEnumerator Execute();

        protected virtual void OnAttackEnded(bool interrupted)
        {
            _endReceiver?.OnAttackEnded(interrupted);
            _endReceiver = null;
        }
    }
}