using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowStagger : WeakHollowBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-staggered", true);
            _timer = Timer.Start(Context.LastHitInfo.StaggerTime, Timeout);
        }

        public override void UpdateState()
        {
            _timer.Tick(Time.deltaTime);
        }

        private void Timeout()
        {
            if (Random.value < 0.3)
            {
                SwitchState<WeakHollowAttack>();
            }
            else
            {
                SwitchState<WeakHollowRetreat>();
            }
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-staggered", false);
        }
    }
}