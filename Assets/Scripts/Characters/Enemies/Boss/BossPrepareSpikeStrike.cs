using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossPrepareSpikeStrike : BossBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            Context.Animator.PlayPrepareSpikeStrikeAnimation();
            Context.VelocityMovement.Stop();

            _timer = Timer.Start(Context.SpikeStrikePrepareTime, OnTimeout);
        }

        public override void UpdateState()
        {
            _timer?.Tick(Time.deltaTime);
        }

        private void OnTimeout()
        {
            SwitchState<BossSpikeStrike>();
        }
    }
}