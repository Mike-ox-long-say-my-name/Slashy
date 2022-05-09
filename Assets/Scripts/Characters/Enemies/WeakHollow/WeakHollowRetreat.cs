using Core;
using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies
{
    public class WeakHollowRetreat : WeakHollowBaseState
    {
        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-walking", true);

            Context.AutoMovement.SetSpeedMultiplier(0.4f);
            Context.AutoMovement.LockRotationOn(Context.PlayerInfo.Transform);

            _timer = Timer.Start(2, SwitchState<WeakHollowIdle>);
        }

        private Timer _timer;

        public override void UpdateState()
        {
            var playerDirection = (Context.PlayerPosition - Context.BaseMovement.Transform.position).WithZeroY();
            playerDirection.Normalize();
            
            Context.VelocityMovement.Move(-playerDirection);

            _timer.Tick(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.AutoMovement.ResetSpeedMultiplier();
            Context.VelocityMovement.Stop();
            Context.AutoMovement.UnlockRotation();
            Context.AnimatorComponent.SetBool("is-walking", false);
        }
    }
}