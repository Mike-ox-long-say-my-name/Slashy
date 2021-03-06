using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowStagger : WeakHollowBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            Context.Animator.StartStaggerAnimation();
            _timer = Timer.Start(Context.LastHitInfo.StaggerTime, Timeout);
        }

        public override void UpdateState()
        {
            _timer.Tick(Time.deltaTime);
        }

        private void Timeout()
        {
            if (Random.value < Context.PursueAfterStaggerChance)
            {
                SwitchState<WeakHollowPursue>();
            }
            else
            {
                SwitchState<WeakHollowRetreat>();
            }
        }

        public override void ExitState()
        {
            Context.Animator.EndStaggerAnimation();
        }
    }
}