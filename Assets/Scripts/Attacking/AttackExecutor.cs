using System;
using System.Collections;
using Core;
using UnityEngine;

namespace Attacking
{
    public abstract class AttackExecutor : MonoBehaviour
    {
        private Coroutine _activeAttack;
        private Action<bool> _attackEnded;

        [SerializeField] private AttackHitbox hitbox;

        public AttackHitbox Hitbox => hitbox;
        public bool IsAttacking => _activeAttack != null;

        private void ResetState()
        {
            _activeAttack = null;
            _attackEnded = null;
        }

        public void InterruptAttack()
        {
            OnAttackEnded(true);
            StopCoroutine(_activeAttack);
            ResetState();
        }

        public void StartExecution(IHitSource source, Action<bool> attackEnded = null)
        {
            if (IsAttacking)
            {
                InterruptAttack();
            }

            _attackEnded = attackEnded;
            _activeAttack = StartCoroutine(ExecuteInternal(source));
        }

        private IEnumerator ExecuteInternal(IHitSource source)
        {
            yield return Execute(source);
            OnAttackEnded(false);
            ResetState();
        }

        protected abstract IEnumerator Execute(IHitSource source);

        protected virtual void OnAttackEnded(bool interrupted)
        {
            _attackEnded?.Invoke(interrupted);
            hitbox.DisableAndClear();
        }
    }
}