using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossWaitAfterAttack : BossBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            _timer = Timer.Start(Random.Range(0.2f, 1f), () => SwitchState<BossPursue>());
        }

        public override void UpdateState()
        {
            _timer?.Tick(Time.deltaTime);
        }
    }
}