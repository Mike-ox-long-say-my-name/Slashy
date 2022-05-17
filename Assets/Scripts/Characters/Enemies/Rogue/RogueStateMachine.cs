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

namespace Characters.Enemies.Rogue
{
    public class RogueBaseState : EnemyBaseState<RogueStateMachine>
    {
        public override void OnHitReceived(HitInfo info)
        {
            Context.LastHit = info;
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.LastHit = info;
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<RogueStaggered>();
        }

        public override void OnDeath(HitInfo info)
        {
            Context.LastHit = info;
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<RogueDeath>();
        }
    }

    public class RogueDeath : EnemyBaseState<RogueStateMachine>
    {
        public override void EnterState()
        {
            BorderManager.Instance.DecreaseAggroCounter();
            Context.AutoMovement.ResetState();
            Context.AutoMovement.UnlockRotation();

            Context.Hurtbox.Disable();
            Context.Animator.SetTrigger("death");
            Context.Destroyable.DestroyLater();
        }
    }

    public class RogueIdle : RogueBaseState
    {
        public override void UpdateState()
        {
            var player = Context.PlayerPosition.WithZeroY();
            var position = Context.transform.position.WithZeroY();
            const float aggroDistance = 8f;
            if (Vector3.Distance(position, player) < aggroDistance)
            {
                BorderManager.Instance.IncreaseAggroCounter();
                SwitchState<RoguePursue>();
            }
        }
    }

    public class RogueStaggered : RogueBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetBool("is-staggered", true);
        }

        public override void OnStaggerEnded()
        {
            var value = Random.value;
            if (value < 0.2f)
            {
                SwitchState<RogueThrust>();
            }
            else if (value < 0.5f)
            {
                SwitchState<RoguePursue>();
            }
            else
            {
                SwitchState<RogueRetreat>();
            }
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-staggered", false);
        }
    }

    public class RogueRetreat : RogueBaseState
    {
        private void SetRandomSpeedMultiplier()
        {
            var multiplier = Random.Range(0.6f, 1.2f);
            Context.AutoMovement.SetSpeedMultiplier(multiplier);
        }

        private void SetRandomRetreatTarget()
        {
            const float maxOffset = 0.8f;
            var direction = new Vector3(Random.Range(0f, maxOffset), 0, Random.Range(0f, maxOffset)).normalized;
            var distance = Random.Range(1.4f, 3.5f);
            Context.AutoMovement.MoveTo(direction * distance);
        }

        public override void EnterState()
        {
            Context.Animator.SetBool("is-walking", true);
            Context.AutoMovement.LockRotationOn(Context.PlayerInfo.Transform);
            Context.AutoMovement.TargetReached += OnTargetReached;

            SetRandomSpeedMultiplier();
            SetRandomRetreatTarget();
            Context.AutoMovement.SetMaxMoveTime(1.5f);
        }

        private void OnTargetReached()
        {
            var value = Random.value;
            if (value < 0.2f)
            {
                SwitchState<RogueWait>();
            }
            else if (value < 0.3f)
            {
                SwitchState<RogueJumpAtPlayer>();
            }
            else
            {
                SwitchState<RoguePursue>();
            }
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-walking", false);

            Context.VelocityMovement.Stop();
            Context.AutoMovement.ResetState();
            Context.AutoMovement.UnlockRotation();
        }
    }

    public class RoguePursue : RogueBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetBool("is-walking", true);

            var player = Context.PlayerInfo.Transform;
            Context.AutoMovement.TargetReached += OnTargetReached;
            Context.AutoMovement.MoveTo(player);
            Context.AutoMovement.LockRotationOn(player);
            Context.AutoMovement.SetTargetReachedEpsilon(1);
        }

        private void OnTargetReached()
        {
            var value = Random.value;
            if (value < 0.2f)
            {
                SwitchState<RogueJumpAway>();
            }
            else if (value < 0.8f)
            {
                SwitchState<RogueThrust>();
            }
            else
            {
                SwitchState<RogueTripleSlash>();
            }
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-walking", false);

            Context.VelocityMovement.Stop();
            Context.AutoMovement.ResetState();
            Context.AutoMovement.UnlockRotation();
        }
    }

    public class RogueTripleSlash : RogueBaseState
    {
        public override void EnterState()
        {
            Context.AutoMovement.UnlockRotation();
            Context.AutoMovement.ResetState();
            Context.VelocityMovement.Stop();

            Context.Animator.SetTrigger("triple-slash");
            Context.TripleSlashExecutor.StartAttack(OnAttackEnded);
            Context.Character.Balance.Frozen = true;
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasInterrupted)
            {
                return;
            }

            var value = Random.value;
            if (value < 0.2)
            {
                SwitchState<RogueWait>();
            }
            else if (value < 0.35f)
            {
                SwitchState<RogueJumpAway>();
            }
            else if (value < 0.7f)
            {
                SwitchState<RogueRetreat>();
            }
            else if (value < 0.85f)
            {
                SwitchState<RogueThrust>();
            }
            else
            {
                SwitchState<RogueTripleSlash>();
            }
        }

        public override void ExitState()
        {
            Context.Character.Balance.Frozen = false;
        }
    }

    public class RogueThrust : RogueBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetTrigger("thrust");
            Context.ThrustExecutor.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (Random.value < Context.JumpAwayAfterThrustChance)
            {
                SwitchState<RogueJumpAway>();
            }
            else
            {
                SwitchState<RoguePursue>();
            }
        }
    }

    public class RogueJumpAtPlayer : RogueBaseState
    {
        private bool _updatePassed;
        private Vector3 _direction;
        private const float Multiplier = 1.5f;

        public override void EnterState()
        {
            Context.Animator.SetBool("is-jumping", true);

            _updatePassed = false;

            Context.AutoMovement.UnlockRotation();
            Context.VelocityMovement.AutoResetVelocity = false;

            var targetPosition = Context.PlayerPosition;
            if (Context.TryPredictPlayerMovement)
            {
                var predictedMovement = GetPredictedPlayerMovement(Multiplier);
                targetPosition += predictedMovement;
            }

            _direction = (targetPosition.WithZeroY() - Context.transform.position.WithZeroY()).normalized;

            // Прогревочный
            Context.VelocityMovement.Move(_direction * Multiplier);
            Context.JumpHandler.Jump();
        }

        private Vector3 GetPredictedPlayerMovement(float multiplier)
        {
            var distance = Vector3.Distance(Context.PlayerPosition.WithZeroY(), Context.transform.position.WithZeroY());
            return Context.PlayerInfo.VelocityMovement.Velocity.WithZeroY().normalized * Mathf.Sqrt(distance * multiplier);
        }

        public override void UpdateState()
        {
            if (_updatePassed && Context.VelocityMovement.BaseMovement.IsGrounded)
            {
                SwitchState<RogueWait>();
            }
            else if (Vector3.Distance(Context.PlayerPosition.WithZeroY(), Context.transform.position.WithZeroY()) < 1)
            {
                Context.VelocityMovement.ResetGravity();
                SwitchState<RogueJumpAttack>();
            }
            else
            {
                Context.VelocityMovement.Move(_direction * Multiplier);
            }

            _updatePassed = true;
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.LastHit = info;
            SwitchState<RogueAirStagger>();
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-jumping", false);
            Context.VelocityMovement.Stop();

            Context.VelocityMovement.AutoResetVelocity = true;
        }
    }

    public class RogueJumpAttack : RogueBaseState
    {
        private bool _updatePassed;

        public override void EnterState()
        {
            Context.Animator.SetTrigger("jump-attack");
            _updatePassed = false;
            Context.JumpAttackExecutor.StartAttack(OnAttackEnded);
        }

        public override void UpdateState()
        {
            if (_updatePassed && Context.BaseMovement.IsGrounded)
            {
                if (Context.JumpAttackExecutor.IsAttacking)
                {
                    Context.JumpAttackExecutor.InterruptAttack();
                }
                SwitchState<RogueWaitAfterJumpAttack>();
            }

            _updatePassed = true;
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasCompleted)
            {
                Context.Animator.SetBool("is-jumping-away", true);
            }
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            Context.LastHit = info;
            SwitchState<RogueAirStagger>();
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-jumping-away", false);
        }
    }

    public class RogueAirStagger : RogueBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            Context.AutoMovement.UnlockRotation();
            Context.Animator.SetBool("is-staggered", true);
            const float airStaggerTime = 3f;
            _timer = Timer.Start(airStaggerTime, SwitchState<RoguePursue>);
        }

        public override void UpdateState()
        {
            _timer.Tick(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-staggered", false);
        }
    }

    public class RogueJumpAway : RogueBaseState
    {
        private bool _updatePassed;

        public override void EnterState()
        {
            Context.Animator.SetBool("is-jumping-away", true);

            var jumpLocation = GetRandomJumpLocation();
            // Прогревочный
            Context.VelocityMovement.Move((jumpLocation - Context.transform.position).normalized);
            Context.JumpHandler.Jump();
            _updatePassed = false;

            Context.VelocityMovement.AutoResetVelocity = false;

            Context.AutoMovement.TargetReached += OnTargetReached;
            Context.AutoMovement.MoveTo(jumpLocation);
            Context.AutoMovement.LockRotationOn(Context.PlayerInfo.Transform);
            Context.AutoMovement.SetMaxMoveTime(1);
            Context.AutoMovement.SetSpeedMultiplier(1);
        }

        public override void UpdateState()
        {
            if (_updatePassed && Context.BaseMovement.IsGrounded)
            {
                OnTargetReached();
            }

            _updatePassed = true;
        }

        private void OnTargetReached()
        {
            if (Random.value < Context.ThrowKnifeChance)
            {
                SwitchState<RoguePursue>();
                // TODO: SwitchState<RogueThrowKnife>();
            }
            else
            {
                SwitchState<RogueWait>();
            }
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.LastHit = info;
            SwitchState<RogueAirStagger>();
        }

        private Vector3 GetRandomJumpLocation()
        {
            var distance = Random.Range(Context.MinJumpAwayDistance, Context.MaxJumpAwayDistance);
            var sign = Mathf.Sign(Context.PlayerPosition.x - Context.transform.position.x);
            var zOffset = Random.Range(0f, Context.MaxJumpAwayZRatio);
            var location = new Vector3(sign, 0, zOffset).normalized;
            return location * distance;
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-jumping-away", false);
            Context.AutoMovement.ResetState();

            Context.VelocityMovement.AutoResetVelocity = true;
        }
    }

    public class RogueWaitAfterJumpAttack : RogueBaseState
    {
        private Timer _waitTimer;

        public override void EnterState()
        {
            var waitTime = Random.Range(0.6f, 1.3f);
            _waitTimer = Timer.Start(waitTime, OnTimeout);
        }

        public override void UpdateState()
        {
            _waitTimer?.Tick(Time.deltaTime);
        }

        private void OnTimeout()
        {
            var value = Random.value;
            if (value < 0.3f)
            {
                SwitchState<RogueThrust>();
            }
            else if (value < 0.6f)
            {
                SwitchState<RogueRetreat>();
            }
            else if (value < 0.75f)
            {
                SwitchState<RogueJumpAtPlayer>();
            }
            else
            {
                SwitchState<RogueJumpAway>();
            }
        }
    }

    public class RogueWait : RogueBaseState
    {
        private Timer _waitTimer;

        public override void EnterState()
        {
            var waitTime = Random.Range(0.3f, 1.7f);
            _waitTimer = Timer.Start(waitTime, OnTimeout);
        }

        public override void UpdateState()
        {
            _waitTimer?.Tick(Time.deltaTime);
        }

        private void OnTimeout()
        {
            if (Random.value < 0.8f
                && Vector3.Distance(Context.PlayerPosition.WithZeroY(), Context.transform.position.WithZeroY()) > 1)
            {
                SwitchState<RogueJumpAtPlayer>();
            }
            else
            {
                SwitchState<RoguePursue>();
            }
        }
    }

    public class RogueThrowKnife : RogueBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetTrigger("throw-knife");
            Context.ThrowKnifeExecutor.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            SwitchState<RogueWait>();
        }
    }

    [RequireComponent(typeof(MixinAutoMovement))]
    [RequireComponent(typeof(MixinAttackExecutorHelper))]
    [RequireComponent(typeof(MixinJumpHandler))]
    public class RogueStateMachine : EnemyStateMachine<RogueStateMachine>
    {
        [SerializeField] private MonoAbstractAttackExecutor tripleSlashExecutor;
        [SerializeField] private MonoAbstractAttackExecutor thrustExecutor;
        [SerializeField] private MonoAbstractAttackExecutor throwKnifeExecutor;
        [SerializeField] private MonoAbstractAttackExecutor jumpAttackExecutor;
        
        [SerializeField, Range(0, 1)] private float jumpAwayAfterThrustChance = 0.7f;
        [SerializeField, Min(0)] private float maxJumpAwayDistance = 8f;
        [SerializeField, Min(0)] private float minJumpAwayDistance = 4f;
        [SerializeField, Min(0)] private float maxJumpAwayZRatio = 0.5f;
        [SerializeField, Range(0, 1)] private float throwKnifeChance = 0.5f;
        [SerializeField] private bool tryPredictPlayerMovement = true;

        public MixinAttackExecutorHelper AttackExecutorHelper { get; private set; }
        public IAutoMovement AutoMovement { get; private set; }

        public IAttackExecutor TripleSlashExecutor => tripleSlashExecutor.GetExecutor();
        public IAttackExecutor ThrustExecutor => thrustExecutor.GetExecutor();
        public IAttackExecutor ThrowKnifeExecutor => throwKnifeExecutor.GetExecutor();
        public IAttackExecutor JumpAttackExecutor => jumpAttackExecutor.GetExecutor();
        
        public float JumpAwayAfterThrustChance => jumpAwayAfterThrustChance;
        public float MaxJumpAwayDistance => maxJumpAwayDistance;
        public float MinJumpAwayDistance => minJumpAwayDistance;
        public float MaxJumpAwayZRatio => maxJumpAwayZRatio;
        public float ThrowKnifeChance => throwKnifeChance;
        public IJumpHandler JumpHandler { get; private set; }
        public bool TryPredictPlayerMovement => tryPredictPlayerMovement;

        protected override EnemyBaseState<RogueStateMachine> StartState()
        {
            var state = new RogueIdle();
            state.Init(this, this);
            return state;
        }

        protected override void Awake()
        {
            base.Awake();

            AttackExecutorHelper = GetComponent<MixinAttackExecutorHelper>();
            AutoMovement = GetComponent<MixinAutoMovement>().AutoMovement;
            JumpHandler = GetComponent<MixinJumpHandler>().JumpHandler;
        }
    }
}