using Characters.Enemies.States;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Modules;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    [RequireComponent(typeof(MixinAutoMovement))]
    [RequireComponent(typeof(MixinJumpHandler))]
    public class RogueStateMachine : EnemyStateMachine<RogueStateMachine>
    {
        [SerializeField] private MonoAbstractAttackExecutor tripleSlashExecutor;
        [SerializeField] private MonoAbstractAttackExecutor thrustExecutor;
        [SerializeField] private MonoAbstractAttackExecutor jumpAttackExecutor;

        [SerializeField, Range(0, 1)] private float jumpAwayAfterThrustChance = 0.7f;
        [SerializeField, Min(0)] private float maxJumpAwayDistance = 8f;
        [SerializeField, Min(0)] private float minJumpAwayDistance = 4f;
        [SerializeField, Min(0)] private float maxJumpAwayZRatio = 0.5f;
        [SerializeField] private bool tryPredictPlayerMovement = true;

        public IAutoMovement AutoMovement { get; private set; }

        public IAttackExecutor TripleSlashExecutor => tripleSlashExecutor.GetExecutor();
        public IAttackExecutor ThrustExecutor => thrustExecutor.GetExecutor();
        public IAttackExecutor JumpAttackExecutor => jumpAttackExecutor.GetExecutor();

        public float JumpAwayAfterThrustChance => jumpAwayAfterThrustChance;
        public float MaxJumpAwayDistance => maxJumpAwayDistance;
        public float MinJumpAwayDistance => minJumpAwayDistance;
        public float MaxJumpAwayZRatio => maxJumpAwayZRatio;
        public IJumpHandler JumpHandler { get; private set; }
        public bool TryPredictPlayerMovement => tryPredictPlayerMovement;
        public RogueAnimator Animator { get; private set; }

        protected override EnemyBaseState<RogueStateMachine> StartState()
        {
            var state = new RogueIdle();
            state.Init(this);
            return state;
        }

        protected override void Awake()
        {
            base.Awake();
            var baseAnimator = GetComponent<Animator>();
            Animator = new RogueAnimator(baseAnimator);

            AutoMovement = GetComponent<MixinAutoMovement>().AutoMovement;
            JumpHandler = GetComponent<MixinJumpHandler>().JumpHandler;
        }
    }
}