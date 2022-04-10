using UnityEngine;

namespace Player.States
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Context.Animator.SetTrigger("idle");
        }

        public override void UpdateState()
        {
            CheckStateSwitch();
        }

        public override void ExitState()
        {
        }

        private void CheckStateSwitch()
        {
            if (Context.MoveInput.sqrMagnitude > 0)
            {
                SwitchState(Factory.Walk());
            }
        }
    }
}