using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerFallState : PlayerAirboneState
    {
        public override void UpdateState()
        {
            ApplyMoveInput();

            CheckStateSwitch();
        }

        private void CheckStateSwitch()
        {
            if (Context.VelocityMovement.BaseMovement.IsGrounded)
            {
                Context.VelocityMovement.Stop();
                SwitchState<PlayerGroundedState>();
            }
            else if (Context.ShouldLightAttack
                     && !Context.AttackedThisAirTime)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerAirLightAttackState>();
            }
        }

        public override void OnWarpStarted(Vector3 target)
        {
            base.OnWarpStarted(target);
            SwitchState<PlayerMovingToFromExternalEvent>();
        }

        public override void OnWarpEnded(Vector3 target)
        {
            base.OnWarpStarted(target);
            SwitchState<PlayerMovingToFromExternalEvent>();
        }
    }
}