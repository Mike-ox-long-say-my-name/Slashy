using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-walking", true);
        }

        public override void UpdateState()
        {
            if (Context.IsGroundState)
            {
                Context.Movement.Move(Context.MoveInput);
            }
            CheckStateSwitch();
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-walking", false);
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