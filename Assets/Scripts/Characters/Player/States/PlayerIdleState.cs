namespace Characters.Player.States
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            if (Context.IsGroundState)
            {
                Context.AnimatorComponent.SetTrigger("idle");
            }
        }

        public override void UpdateState()
        {
            CheckStateSwitch();
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