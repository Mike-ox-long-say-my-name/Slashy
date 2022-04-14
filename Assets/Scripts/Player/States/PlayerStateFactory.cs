namespace Player.States
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

        public PlayerBaseState Walk()
        {
            return new PlayerWalkState(_stateMachine, this);
        }

        public PlayerBaseState Idle()
        {
            return new PlayerIdleState(_stateMachine, this);
        }

        public PlayerBaseState Attack()
        {
            return new PlayerAttackState(_stateMachine, this);
        }
    }
}