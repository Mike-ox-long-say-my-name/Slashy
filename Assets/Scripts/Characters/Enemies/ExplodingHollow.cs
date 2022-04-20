using Attacking;
using Characters.Enemies.States;
using UnityEngine;

namespace Characters.Enemies
{
    public class ExplodingHollowIdle : EnemyBaseState<ExplodingHollow>
    {
        public override void UpdateState()
        {
            if (Vector3.Distance(Context.PlayerPosition, Context.transform.position) < 4)
            {
                SwitchState(Factory.Pursue());
            }
        }
    }

    public class ExplodingHollowPursue : EnemyBaseState<ExplodingHollow>
    {
        public override void UpdateState()
        {
            var player = Context.PlayerPosition;
            var self = Context.transform.position;
            if (Vector3.Distance(player, self) > 2)
            {
                var direction = player - self;
                direction.y = 0;
                direction.Normalize();

                Context.Movement.Move(new Vector2(direction.x, direction.z));
            }
            else
            {
                SwitchState(Factory.Attack());
            }
        }
    }

    public class ExplodingHollowAttack : EnemyBaseState<ExplodingHollow>
    {
        public override void EnterState()
        {
            Context.Movement.ResetXZVelocity();
            if (Context.HasExplosionAttackExecutor)
            {
                Context.ExplosionAttackExecutor.StartExecution(Context.Character,
                    _ => SwitchState(Factory.Idle()));
            }
        }
    }

    public class ExplodingHollowStagger : EnemyBaseState<ExplodingHollow>
    {
        public override void EnterState()
        {
            SwitchState(Factory.Pursue());
        }
    }

    public class ExplodingHollow : EnemyStateMachine<ExplodingHollow>
    {
        [SerializeField] private AttackExecutor explosionAttackExecutor;

        public AttackExecutor ExplosionAttackExecutor => explosionAttackExecutor;
        public bool HasExplosionAttackExecutor { get; private set; } = true;

        protected override void Awake()
        {
            base.Awake();
            if (explosionAttackExecutor == null)
            {
                Debug.LogWarning("Explosion Attack Executor is not assigned", this);
                HasExplosionAttackExecutor = false;
            }
        }

        protected override EnemyStateFactory<ExplodingHollow> CreateFactory()
        {
            return new EnemyStateFactoryBuilder<ExplodingHollow>()
                .Idle<ExplodingHollowIdle>()
                .Pursue<ExplodingHollowPursue>()
                .Attack<ExplodingHollowAttack>()
                .Stagger<ExplodingHollowStagger>()
                .Build(this, this);
        }
    }
}
