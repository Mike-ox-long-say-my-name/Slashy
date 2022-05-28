using Characters.Enemies.States;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Modules;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    [RequireComponent(typeof(MixinBossEventDispatcher))]
    public class BossStateMachine : EnemyStateMachine<BossStateMachine>
    {
        [SerializeField] private MonoAbstractAttackExecutor spikeStrikeExecutor;
        [SerializeField] private MonoAbstractAttackExecutor horizontalSwingExecutor;
        [SerializeField] private MonoAbstractAttackExecutor jumpAttackExecutor;
        [SerializeField] private BossThrustAttackExecutor thrustAttackExecutor;

        [SerializeField] private float spikeStrikePrepareTime;
        [SerializeField, Range(0, 1)] private float spikeStrikeRepeatChance;
        [SerializeField] private float maxDashDistance = 7f;
        [SerializeField, Range(0, 1)] private float thrustAfterSwingChance = 0.25f;
        [SerializeField] private int maxSpikeStrikesInRow = 3;

        public float MaxDashDistance => maxDashDistance;

        public MixinBossEventDispatcher BossEvents { get; private set; }
        public IAutoMovement AutoMovement { get; private set; }
        public IJumpHandler JumpHandler { get; private set; }

        public int MaxSpikeStrikesInRow => maxSpikeStrikesInRow;
        public BossAnimator Animator { get; private set; }
        public IAttackExecutor SpikeStrikeExecutor => spikeStrikeExecutor.GetExecutor();
        public IAttackExecutor HorizontalSwingExecutor => horizontalSwingExecutor.GetExecutor();
        public IAttackExecutor JumpAttackExecutor => jumpAttackExecutor.GetExecutor();

        public BossThrustAttackExecutor ThrustAttackConfigurator => thrustAttackExecutor;
        public IAttackExecutor ThrustAttackExecutor => thrustAttackExecutor.GetExecutor();

        public float SpikeStrikePrepareTime => spikeStrikePrepareTime;

        public float SpikeStrikeRepeatChance => spikeStrikeRepeatChance;
        public float ThrustAfterSwingChance => thrustAfterSwingChance;

        protected override EnemyBaseState<BossStateMachine> StartState()
        {
            var state = new BossIdle();
            state.Init(this);
            return state;
        }

        protected override void Awake()
        {
            base.Awake();

            var baseAnimator = GetComponent<Animator>();
            Animator = new BossAnimator(baseAnimator);

            BossEvents = GetComponent<MixinBossEventDispatcher>();
            AutoMovement = GetComponent<MixinAutoMovement>().AutoMovement;
            JumpHandler = GetComponent<MixinJumpHandler>().JumpHandler;
        }
    }
}