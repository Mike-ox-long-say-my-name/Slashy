using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerTouchingBonfireState : PlayerBaseState
    {
        private Timer _touchTimer;

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            Context.Hurtbox.Disable();
            Context.AnimatorComponent.SetBool("is-saving", true);

            const float animationTime = 1.5f;
            _touchTimer = Timer.Start(animationTime, SwitchState<PlayerGroundedState>);
        }

        public override void UpdateState()
        {
            _touchTimer.Tick(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.Hurtbox.Enable();
            Context.AnimatorComponent.SetBool("is-saving", false);
        }
    }
}