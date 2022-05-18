using Characters.Enemies.States;
using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Modules;
using Core.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters.Enemies.Boss
{
    public class BossBaseState : EnemyBaseState<BossStateMachine>
    {
        public override void OnStaggered(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<BossStaggered>();
        }

        public override void OnDeath(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<BossDeath>();
        }
    }

    public class BossDeath : BossBaseState
    {
        public override void EnterState()
        {
            Debug.Log("He's dead");
            Context.BossEvents.Died?.Invoke();
            BorderManager.Instance.DecreaseAggroCounter();
            Context.Destroyable.DestroyLater();
        }
    }

    public class BossStaggered : BossBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            const float staggerTime = 3.5f;
            _timer = Timer.Start(staggerTime, OnTimeout);
        }

        public override void UpdateState()
        {
            _timer?.Tick(Time.deltaTime);
        }

        private void OnTimeout()
        {
            SwitchState<BossPursue>();
            // TODO
        }
    }

    public class BossIdle : BossBaseState
    {
        public override void UpdateState()
        {
            var distance = Vector3.Distance(Context.PlayerPosition.WithZeroY(), Context.transform.position.WithZeroY());
            if (distance < 10f)
            {
                Context.BossEvents.FightStarted?.Invoke();
                BorderManager.Instance.IncreaseAggroCounter();
                SwitchState<BossPursue>();
            }
        }
    }

    public class BossPursue : BossBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetBool("is-walking", true);

            var player = Context.PlayerInfo.Transform;
            Context.AutoMovement.TargetReached += OnTargetReached;
            Context.AutoMovement.MoveTo(player);
            Context.AutoMovement.LockRotationOn(player);
            Context.AutoMovement.SetTargetReachedEpsilon(2.5f);
        }

        private void OnTargetReached()
        {
            var value = Random.value;
            SwitchState<BossSpikeStrike>();
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-walking", false);

            Context.VelocityMovement.Stop();
            Context.AutoMovement.ResetState();
            Context.AutoMovement.UnlockRotation();
        }
    }

    public class BossSpikeStrike : BossBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetTrigger("spike-strike");
            Context.SpikeStrikeExecutor.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasInterrupted)
            {
                return;
            }

            SwitchState<BossPursue>();
        }
    }

    [RequireComponent(typeof(MixinBossEventDispatcher))]
    public class BossStateMachine : EnemyStateMachine<BossStateMachine>
    {
        [SerializeField] private MonoAbstractAttackExecutor spikeStrikeExecutor;

        public MixinBossEventDispatcher BossEvents { get; private set; }
        public MixinAttackExecutorHelper AttackExecutorHelper { get; private set; }
        public IAutoMovement AutoMovement { get; private set; }
        public IJumpHandler JumpHandler { get; private set; }

        public IAttackExecutor SpikeStrikeExecutor => spikeStrikeExecutor.GetExecutor();

        protected override EnemyBaseState<BossStateMachine> StartState()
        {
            var state = new BossIdle();
            state.Init(this);
            return state;
        }

        protected override void Awake()
        {
            base.Awake();

            BossEvents = GetComponent<MixinBossEventDispatcher>();
            AttackExecutorHelper = GetComponent<MixinAttackExecutorHelper>();
            AutoMovement = GetComponent<MixinAutoMovement>().AutoMovement;
            JumpHandler = GetComponent<MixinJumpHandler>().JumpHandler;
        }
    }
}