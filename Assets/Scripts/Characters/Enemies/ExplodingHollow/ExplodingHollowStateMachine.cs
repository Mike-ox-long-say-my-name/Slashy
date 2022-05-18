using Characters.Enemies.States;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Characters.Enemies.ExplodingHollow
{
    [RequireComponent(typeof(MixinDamageSource))]
    public class ExplodingHollowStateMachine : EnemyStateMachine<ExplodingHollowStateMachine>
    {
        [SerializeField, Min(0)] private float aggroDistance = 5;
        [SerializeField, Min(0)] private float chargeTime = 2;
        [SerializeField, Min(0)] private float dotTickInterval = 0.3f;

        [SerializeField] private MonoAbstractAttackExecutor explosionMonoAttackHandler;
        [SerializeField] private MonoAbstractAttackExecutor punchMonoAttack;
        [SerializeField] private ParticleSystem chargeBurnParticles;

        public float AggroDistance => aggroDistance;
        public float ChargeTime => chargeTime;

        public IAttackExecutor ExplosionAttack => explosionMonoAttackHandler.GetExecutor();
        public IAttackExecutor PunchAttack => punchMonoAttack.GetExecutor();
        
        public float DotTickInterval => dotTickInterval;
        public IPushable Pushable { get; private set; }

        public ParticleSystem ChargeBurnParticles => chargeBurnParticles;

        public bool WasHitByPlayer { get; set; }
        public DamageStats DamageStats { get; private set; }

        protected override EnemyBaseState<ExplodingHollowStateMachine> StartState()
        {
            var state = new ExplodingHollowIdle();
            state.Init(this);
            return state;
        }

        protected override void Awake()
        {
            base.Awake();

            Pushable = GetComponent<MixinPushable>().Pushable;
            DamageStats = GetComponent<MixinDamageSource>().DamageStats;

            if (punchMonoAttack == null)
            {
                Debug.LogWarning("Punch Attack Executor is not assigned", this);
                enabled = false;
            }

            if (explosionMonoAttackHandler == null)
            {
                Debug.LogWarning("Explosion Attack Executor is not assigned", this);
                enabled = false;
            }
        }
    }
}
