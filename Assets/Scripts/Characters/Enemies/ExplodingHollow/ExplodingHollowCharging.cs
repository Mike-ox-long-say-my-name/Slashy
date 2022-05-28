using Core.Attacking;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowCharging : ExplodingHollowBaseState
    {
        private Timer _prepareTimer;

        public override void EnterState()
        {
            Context.Character.Balance.Frozen = true;

            Context.Animator.PlayChargeAnimation();
            _prepareTimer = Timer.Start(Context.ChargeTime, () => SwitchState<ExplodingHollowRunning>());
        }

        public override void UpdateState()
        {
            _prepareTimer.Tick(Time.deltaTime);
        }

        public override void OnHitReceived(HitInfo info)
        {
        }
    }
}