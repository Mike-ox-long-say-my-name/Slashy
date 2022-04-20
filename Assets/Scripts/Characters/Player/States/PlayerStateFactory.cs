namespace Characters.Player.States
{
    public class PlayerStateFactory
    {
        private readonly PlayerStateMachine _stateMachine;

        public PlayerStateFactory(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public PlayerBaseState Grounded()
        {
            return new PlayerGroundedState(_stateMachine, this);
        }

        public PlayerBaseState Dash()
        {
            return new PlayerDashState(_stateMachine, this);
        }

        public PlayerBaseState Jump()
        {
            return new PlayerJumpState(_stateMachine, this);
        }

        public PlayerBaseState Fall()
        {
            return new PlayerFallState(_stateMachine, this);
        }

        public PlayerBaseState GroundLightAttack()
        {
            return new PlayerGroundLightAttackState(_stateMachine, this);
        }

        public PlayerHealState Heal()
        {
            return new PlayerHealState(_stateMachine, this);
        }

        public PlayerGroundStaggerState GroundStagger()
        {
            return new PlayerGroundStaggerState(_stateMachine, this);
        }

        public PlayerAirboneStaggerState AirboneStagger()
        {
            return new PlayerAirboneStaggerState(_stateMachine, this);
        }
    }
}