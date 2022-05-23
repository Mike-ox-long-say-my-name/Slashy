using Core.Characters.Interfaces;

namespace Characters.Enemies.Boss
{
    public class BossJumpAttackStart : BossBaseState
    {
        private bool _updated;

        public override void EnterState()
        {
            Context.Animator.SetTrigger("jump-start");
            Context.Character.Balance.Frozen = true;

            Context.VelocityMovement.Stop();
            Context.JumpHandler.Jump();

            _updated = false;
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
            Context.Character.Balance.Frozen = false;
        }
    }
}