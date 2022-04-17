using UnityEngine;

namespace Player.States
{
    public class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Context.AnimatorComponent.SetTrigger("walk");
        }

        public override void UpdateState()
        {
            Context.Movement.Move(Context.MoveInput);
            CheckStateSwitch();
        }

        private void CheckStateSwitch()
        {
            if (Mathf.Approximately(Context.MoveInput.sqrMagnitude, 0))
            {
                SwitchState(Factory.Idle());
            }
        }
    }
}