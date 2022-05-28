using Characters.Enemies.States;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MixinAutoMovement))]
    public class WeakHollowStateMachine : EnemyStateMachine<WeakHollowStateMachine>
    {
        [SerializeField, Range(0, 1)] private float attackRepeatChance = 0.2f;
        [SerializeField, Range(0, 1)] private float pursueAfterStaggerChance = 0.3f;
        [SerializeField] private bool tryToSurroundPlayer = true;
        [SerializeField] private MonoAbstractAttackExecutor punchAttack;

        public float AttackRepeatChance => attackRepeatChance;
        public float PursueAfterStaggerChance => pursueAfterStaggerChance;

        public bool TryToSurroundPlayer => tryToSurroundPlayer;

        public HitInfo LastHitInfo { get; set; }
        public WeakHollowAnimator Animator { get; private set; }

        public IAttackExecutor PunchAttackExecutor => punchAttack.GetExecutor();
        public IAutoMovement AutoMovement { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            var baseAnimator = GetComponent<Animator>();
            Animator = new WeakHollowAnimator(baseAnimator);

            AutoMovement = GetComponent<MixinAutoMovement>().AutoMovement;
        }

        protected override EnemyBaseState<WeakHollowStateMachine> StartState()
        {
            var state = new WeakHollowIdle();
            state.Init(this);
            return state;
        }
    }
}