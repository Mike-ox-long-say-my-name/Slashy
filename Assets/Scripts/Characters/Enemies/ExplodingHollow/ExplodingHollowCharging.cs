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
            //Context.VelocityMovement.Stop();

            Context.Character.Balance.Frozen = true;

            Context.Animator.SetTrigger("charge");
            _prepareTimer = Timer.Start(Context.ChargeTime, SwitchState<ExplodingHollowRunning>);
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