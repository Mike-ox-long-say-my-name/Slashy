namespace Characters.Enemies.States
{
    public class EnemyStateFactory
    {
        private readonly EnemyStateMachine _stateMachine;

        public EnemyStateFactory(EnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public EnemyStaggerState Stagger()
        {
            return new EnemyStaggerState(_stateMachine, this);
        }

        public EnemyIdleState Idle()
        {
            return new EnemyIdleState(_stateMachine, this);
        }

        public EnemyPursueState Pursue()
        {
            return new EnemyPursueState(_stateMachine, this);
        }

        public EnemyAttackState Attack()
        {
            return new EnemyAttackState(_stateMachine, this);
        }
    }
}