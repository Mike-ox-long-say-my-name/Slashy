using UnityEngine;

namespace Player.States
{
    public class PlayerAttackState : PlayerBaseState
    {
        public PlayerAttackState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Context.AppliedVelocityX = 0;
            Context.AppliedVelocityZ = 0;
            Context.CanDash = false;
            Context.CanJump = false;

            void AttackEnded(bool _)
            {
                Context.CanDash = true;
                Context.CanJump = true;
                SwitchState(Factory.Idle());
            }

            Context.LightAttackExecutor.StartExecution(Context.PlayerCharacter, AttackEnded);
            Context.Animator.SetTrigger("attack-short");
        }

        public override void UpdateState()
        {
        }

        public override void ExitState()
        {
        }
    }
}