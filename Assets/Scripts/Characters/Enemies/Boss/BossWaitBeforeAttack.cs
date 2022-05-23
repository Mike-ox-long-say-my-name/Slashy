using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossWaitBeforeAttack : BossBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            _timer = Timer.Start(Random.Range(0.05f, 0.2f), RandomAttack);
        }

        public override void UpdateState()
        {
            _timer?.Tick(Time.deltaTime);
        }

        private void RandomAttack()
        {
            var value = Random.value;
            if (value < 0.2f)
            {
                SwitchState<BossPrepareSpikeStrike>();
            }
            else if (value < 0.4f)
            {
                SwitchState<BossHorizontalSwing>();
            }
            else if (value < 0.6f)
            {
                SwitchState<BossThrustWithDash>();
            }
            else if (value < 0.8f)
            {
                SwitchState<BossThrustStationary>();
            }
            else
            {
                SwitchState<BossJumpAttackStart>();
            }
        }
    }
}