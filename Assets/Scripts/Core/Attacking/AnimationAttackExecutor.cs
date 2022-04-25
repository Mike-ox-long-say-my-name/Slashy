using System.Collections;
using Core.Attacking.Interfaces;
using Core.Utilities;

namespace Core.Attacking
{
    public class AnimationAttackExecutor : AttackExecutor, IAttackAnimationEventReceiver
    {
        private readonly Trigger _attackEnded = new Trigger();

        public AnimationAttackExecutor(ICoroutineHost host, IAttackbox attackbox)
            : base(host, attackbox)
        {
        }

        protected override IEnumerator Execute()
        {
            _attackEnded.Reset();
            while (!_attackEnded.CheckAndReset())
            {
                OnAttackTick();
                yield return null;
            }
        }

        protected virtual void OnAttackTick()
        {
        }

        protected override void OnAttackEnded(bool interrupted)
        {
            EndAttack();
            base.OnAttackEnded(interrupted);
        }

        protected void EndAttack()
        {
            _attackEnded.Set();
        }

        public virtual void OnEnableHitbox()
        {
            Attackbox.Enable();
        }

        public virtual void OnDisableHitbox()
        {
            Attackbox.Disable();
        }

        public virtual void OnAnimationEnded()
        {
            EndAttack();
        }

        public void ReceiveEnableHitbox()
        {
            if (IsAttacking)
            {
                OnEnableHitbox();
            }
        }

        public void ReceiveDisableHitbox()
        {
            if (IsAttacking)
            {
                OnDisableHitbox();
            }
        }

        public void ReceiveAnimationEnded()
        {
            if (IsAttacking)
            {
                OnAnimationEnded();
            }
        }
    }
}