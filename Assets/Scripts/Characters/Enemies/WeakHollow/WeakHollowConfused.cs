using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowConfused : WeakHollowBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            _timer = Timer.Start(Random.Range(0.3f, 1.5f), () => SwitchState<WeakHollowPursue>());
        }

        public override void UpdateState()
        {
            _timer.Tick(Time.deltaTime);
        }
    }
}