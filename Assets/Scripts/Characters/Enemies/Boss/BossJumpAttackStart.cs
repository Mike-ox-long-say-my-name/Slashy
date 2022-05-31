using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossJumpAttackStart : BossBaseState
    {
        private bool _updated;

        public override void EnterState()
        {
            Context.Animator.PlayJumpStartAnimation();
            Context.Character.Balance.Frozen = true;

            Context.VelocityMovement.Stop();

            Context.VelocityMovement.AutoResetVelocity = false;
            if (Random.value < 0.4f)
            {
                PrewarmMovementToPlayerDirection();
            }

            Context.JumpHandler.Jump();

            _updated = false;
        }

        private void PrewarmMovementToPlayerDirection()
        {
            var direction = (Context.PlayerPosition - Context.transform.position);
            var distance = direction.magnitude;
            Context.VelocityMovement.Move(direction.normalized * Mathf.Pow(distance, 0.3f));
        }

        public override void UpdateState()
        {
            if (_updated && Context.BaseMovement.IsGrounded)
            {
                SwitchState<BossJumpAttackEnd>();
            }

            _updated = true;
        }

        public override void ExitState()
        {
            Context.VelocityMovement.AutoResetVelocity = true;
            Context.Character.Balance.Frozen = false;
        }
    }
}