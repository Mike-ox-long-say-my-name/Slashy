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
            SwitchState<BossStaggered>(true);
        }

        public override void OnDeath(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<BossDeath>(true);
        }
    }

    public class BossDeath : BossBaseState
    {
        public override void EnterState()
        {
            Debug.Log("He's dead");
            Context.BossEvents.Died?.Invoke();
            FightManager.Instance.DecreaseAggroCounter();
            Context.Destroyable.DestroyLater();
        }

        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
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
            SwitchState<BossWaitAfterAttack>();
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
                FightManager.Instance.IncreaseAggroCounter();
                SwitchState<BossPursue>();
            }
        }
    }

    public class BossHorizontalSwing : BossBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetTrigger("horizontal-swing");
            Context.HorizontalSwingExecutor.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasInterrupted)
            {
                return;
            }

            SwitchState<BossWaitAfterAttack>();
        }
    }

    public class BossPursue : BossBaseState
    {
        private float _pursueTime;

        public override void EnterState()
        {
            Context.Animator.SetBool("is-walking", true);

            var player = Context.PlayerInfo.Transform;
            Context.AutoMovement.TargetReached += OnTargetReached;
            Context.AutoMovement.MoveTo(player);
            Context.AutoMovement.LockRotationOn(player);
            Context.AutoMovement.SetTargetReachedEpsilon(2.5f);
            Context.AutoMovement.SetSpeedMultiplier(Random.Range(1f, 1.8f));

            _pursueTime = 0;
        }

        public override void UpdateState()
        {
            _pursueTime += Time.deltaTime;

            if (_pursueTime > 3)
            {
                if (Random.value < 0.4f)
                {
                    SwitchState<BossJumpAttackStart>();
                }
                else
                {
                    SwitchState<BossPrepareSpikeStrike>();
                }
            }
        }

        private void OnTargetReached()
        {
            SwitchState<BossWaitBeforeAttack>();
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-walking", false);

            Context.VelocityMovement.Stop();
            Context.AutoMovement.ResetState();
            Context.AutoMovement.UnlockRotation();
        }
    }

    public class BossPrepareSpikeStrike : BossBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            Context.Animator.SetTrigger("prepare-spike-strike");
            Context.VelocityMovement.Stop();

            _timer = Timer.Start(Context.SpikeStrikePrepareTime, OnTimeout);
        }

        public override void UpdateState()
        {
            _timer?.Tick(Time.deltaTime);
        }

        private void OnTimeout()
        {
            SwitchState<BossSpikeStrike>();
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

            if (Random.value < Context.SpikeStrikeRepeatChance)
            {
                SwitchState<BossSpikeStrike>();
            }
            else
            {
                SwitchState<BossWaitAfterAttack>();
            }
        }
    }

    public class BossJumpAttackStart : BossBaseState
    {
        private bool _updated;

        public override void EnterState()
        {
            Context.Animator.SetTrigger("jump-start");
            Context.Character.Balance.Frozen = true;

            Context.VelocityMovement.Stop();
            Context.JumpHandler.Jump();

            _updated = false;
        }

        public override void UpdateState()
        {
            if (_updated && Context.BaseMovement.IsGrounded)
            {
                SwitchState<BossJumpAttackEnd>();
            }

            _updated = true;
        }

        public override void ExitState()
        {
            Context.Character.Balance.Frozen = false;
        }
    }

    public class BossJumpAttackEnd : BossBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetTrigger("jump-end");
            Context.JumpAttackExecutor.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasInterrupted)
            {
                return;
            }

            SwitchState<BossWaitAfterAttack>();
        }
    }

    public class BossWaitAfterAttack : BossBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            _timer = Timer.Start(Random.Range(0.2f, 1f), () => SwitchState<BossPursue>());
        }

        public override void UpdateState()
        {
            _timer?.Tick(Time.deltaTime);
        }
    }

    public class BossWaitBeforeAttack : BossBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            _timer = Timer.Start(Random.Range(0.05f, 0.2f), RandomAttack);
        }

        public override void UpdateState()
        {
            _timer?.Tick(Time.deltaTime);
        }

        private void RandomAttack()
        {
            var value = Random.value;
            // TODO: чето адекватное сделать
            if (value < 0.3f)
            {
                SwitchState<BossPrepareSpikeStrike>();
            }
            else if (value < 0.6f)
            {
                SwitchState<BossHorizontalSwing>();
            }
            else
            {
                SwitchState<BossJumpAttackStart>();
            }
        }
    }

    [RequireComponent(typeof(MixinBossEventDispatcher))]
    public class BossStateMachine : EnemyStateMachine<BossStateMachine>
    {
        [SerializeField] private MonoAbstractAttackExecutor spikeStrikeExecutor;
        [SerializeField] private MonoAbstractAttackExecutor horizontalSwingExecutor;
        [SerializeField] private MonoAbstractAttackExecutor jumpAttackExecutor;

        [SerializeField] private float spikeStrikePrepareTime;
        [SerializeField, Range(0, 1)] private float spikeStrikeRepeatChance;

        public MixinBossEventDispatcher BossEvents { get; private set; }
        public MixinAttackExecutorHelper AttackExecutorHelper { get; private set; }
        public IAutoMovement AutoMovement { get; private set; }
        public IJumpHandler JumpHandler { get; private set; }

        public IAttackExecutor SpikeStrikeExecutor => spikeStrikeExecutor.GetExecutor();
        public IAttackExecutor HorizontalSwingExecutor => horizontalSwingExecutor.GetExecutor();
        public IAttackExecutor JumpAttackExecutor => jumpAttackExecutor.GetExecutor();

        public float SpikeStrikePrepareTime => spikeStrikePrepareTime;

        public float SpikeStrikeRepeatChance => spikeStrikeRepeatChance;

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