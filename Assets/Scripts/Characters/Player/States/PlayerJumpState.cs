namespace Characters.Player.States
{
    public class PlayerJumpState : PlayerAirboneState
    {
        public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            Context.AnimatorComponent.SetTrigger("jump");

            Context.Character.SpendStamina(Context.PlayerConfig.JumpStaminaCost);
            Context.Movement.Jump();
        }

        public override void UpdateState()
        {
            HandleControl();

            CheckStateSwitch();
        }

        public virtual void CheckStateSwitch()
        {
            if (Context.Movement.IsGrounded)
            {
                SwitchState(Factory.Grounded());
            }
            else if (Context.Movement.IsFalling)
            {
                SwitchState(Factory.Fall());
            }
            else if (Context.CanStartAttack && Context.Input.IsLightAttackPressed)
            {
                Context.Input.ResetBufferedInput();
                // TODO: SwitchState(Factory.AirLightAttack());
            }
        }
    }
}