using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies
{
    public class WeakHollowConfused : WeakHollowBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            _timer = Timer.Start(3, SwitchState<WeakHollowPursue>);
        }

        public override void UpdateState()
        {
            _timer.Tick(Time.deltaTime);
        }
    }
}