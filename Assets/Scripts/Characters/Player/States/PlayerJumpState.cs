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

            Context.PlayerCharacter.SpendStamina(Context.PlayerConfig.JumpStaminaCost);
            Context.Movement.Jump();
        }

        public override void UpdateState()
        {
            HandleGravity();
            HandleAirboneControl();

            CheckStateSwitch();
        }

        public virtual void CheckStateSwitch()
        {
            if (Context.Movement.IsGrounded)
            {
                SwitchState(Factory.Grounded());
            }
            else if (Context.Movement.Velocity.y < 0)
            {
                SwitchState(Factory.Fall());
            }
            else if (Context.CanStartAttack && Context.Input.IsLightAttackPressed.CheckAndReset())
            {
                // TODO: SwitchState(Factory.AirLightAttack());
            }
        }
    }
}