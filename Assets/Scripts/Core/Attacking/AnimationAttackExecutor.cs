using System.Collections;
using Core.Attacking.Interfaces;
using Core.Utilities;

namespace Core.Attacking
{
    public class AnimationAttackExecutor : AttackExecutor, IAnimationAttackExecutorContext
    {
        private readonly Trigger _attackEnded = new Trigger();

        public IAttackEventHandler EventHandler { get; set; }

        public void UseDefaultHandler()
        {
            EventHandler = new DefaultAttackEventHandler();
        }

        public AnimationAttackExecutor(ICoroutineHost host, IAttackbox attackbox)
            : base(host, attackbox)
        {
        }

        protected override IEnumerator Execute()
        {
            _attackEnded.Reset();
            while (!_attackEnded.CheckAndReset())
            {
                EventHandler?.HandleTick(this);
                yield return null;
            }
        }

        protected override void OnAttackEnded(bool interrupted)
        {
            EndAttack();
            base.OnAttackEnded(interrupted);
        }

        public void EndAttack()
        {
            _attackEnded.Set();
        }

        IAttackbox IAnimationAttackExecutorContext.Attackbox => Attackbox;

        public void OnEnableHitbox()
        {
            EventHandler?.HandleEnableHitbox(this);
        }

        public void OnDisableHitbox()
        {
            EventHandler?.HandleDisableHitbox(this);
        }

        public void OnAnimationEnded()
        {
            EventHandler?.HandleAnimationEnd(this);
            EndAttack();
        }
    }
}