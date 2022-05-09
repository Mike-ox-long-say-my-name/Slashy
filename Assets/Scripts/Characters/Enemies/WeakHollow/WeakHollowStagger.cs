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
            _timer = Timer.Start(Context.LastHitInfo.StaggerTime, SwitchState<WeakHollowIdle>);
        }

        public override void UpdateState()
        {
            _timer.Tick(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-staggered", false);
        }
    }
}