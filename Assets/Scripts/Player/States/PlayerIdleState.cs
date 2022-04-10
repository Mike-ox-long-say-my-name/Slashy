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
            Debug.Log("Enter Idle");
        }

        public override void UpdateState()
        {
            CheckStateSwitch();
        }

        public override void ExitState()
        {
            Debug.Log("Exit Idle");
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