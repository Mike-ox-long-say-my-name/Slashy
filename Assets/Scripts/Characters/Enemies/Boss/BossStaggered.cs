using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossStaggered : BossBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            Context.Animator.SetBool("is-staggered", true);
            const float staggerTime = 3.5f;
            _timer = Timer.Start(staggerTime, OnTimeout);
        }

        public override void UpdateState()
        {
            _timer?.Tick(Time.deltaTime);
        }

        private void OnTimeout()
        {
            SwitchState<BossWaitAfterAttack>();
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-staggered", false);
        }
    }
}