using Characters.Enemies.States;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters;
using Core.Characters.Mono;
using UnityEngine;

namespace Characters.Enemies
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MixinAutoMovement))]
    public class WeakHollowStateMachine : EnemyStateMachine<WeakHollowStateMachine>
    {
        [SerializeField] private MonoAbstractAttackExecutor punchAttack;

        public HitInfo LastHitInfo { get; set; }
        public Animator AnimatorComponent { get; private set; }

        public IAttackExecutor PunchAttackExecutor => punchAttack.GetExecutor();
        public IAutoMovement AutoMovement { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            AnimatorComponent = GetComponent<Animator>();
            AutoMovement = GetComponent<MixinAutoMovement>().AutoMovement;
        }

        protected override EnemyBaseState<WeakHollowStateMachine> StartState()
        {
            var state = new WeakHollowIdle();
            state.Init(this, this);
            return state;
        }

        public void InterruptActiveAttack()
        {
            if (PunchAttackExecutor.IsAttacking)
            {
                PunchAttackExecutor.InterruptAttack();
            }
        }
    }
}