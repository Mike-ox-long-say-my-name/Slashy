using Characters.Enemies.States;
using Core.Attacking;
using Core.Utilities;
using System.Collections;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Player.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters.Enemies
{
    public class ExplodingHollowIdle : ExplodingHollowBaseState
    {
        public override void UpdateState()
        {
            if (Vector3.Distance(Context.PlayerPosition, Context.transform.position) < Context.AggroDistance)
            {
                SwitchState<ExplodingHollowPursue>();
            }
        }
    }

    public class ExplodingHollowPursue : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-walking", true);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-walking", false);
        }

        public override void UpdateState()
        {
            var player = Context.PlayerPosition;
            var self = Context.transform.position;
            var direction = player - self;
            direction.y = 0;
            if (direction.magnitude > 1.2f)
            {
                direction.Normalize();
                Context.VelocityMovement.Move(new Vector3(direction.x, 0, direction.z));
            }
            else
            {
                Context.VelocityMovement.Movement.Rotate(direction.x);
                SwitchState<ExplodingHollowAttack>();
            }
        }
    }

    public abstract class ExplodingHollowBaseState : EnemyBaseState<ExplodingHollow>
    {
        public override void OnDeath(HitInfo info)
        {
            SwitchState<ExplodingHollowDeath>();
        }

        public override void OnHitReceived(HitInfo info)
        {
            if (info.Source.Character is IPlayerCharacter)
            {
                SwitchState<ExplodingHollowCharging>();
            }
        }

        public override void OnStaggered(HitInfo info)
        {
            if (!(info.Source.Character is IPlayerCharacter))
            {
                SwitchState<ExplodingHollowStagger>();
            }
        }
    }

    public class ExplodingHollowAttack : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            Context.AnimatorComponent.SetTrigger("attack");

            Context.PunchAttack.StartAttack(inter =>
            {
                if (!inter)
                {
                    SwitchState<ExplodingHollowPursue>();
                }
            });
        }

        public override void OnDeath(HitInfo info)
        {
            if (Context.PunchAttack.IsAttacking)
            {
                Context.PunchAttack.InterruptAttack();
            }
            base.OnDeath(info);
        }

        public override void OnStaggered(HitInfo info)
        {
            if (Context.PunchAttack.IsAttacking)
            {
                Context.PunchAttack.InterruptAttack();
            }
            base.OnStaggered(info);
        }

        public override void OnHitReceived(HitInfo info)
        {
            if (info.Source.Character is IPlayerCharacter && Context.PunchAttack.IsAttacking)
            {
                Context.PunchAttack.InterruptAttack();
            }
            base.OnHitReceived(info);
        }
    }

    public class ExplodingHollowRunning : ExplodingHollowBaseState
    {
        private readonly TimedTrigger _dotTrigger = new TimedTrigger();

        public override void EnterState()
        {
            _dotTrigger.Reset();

            Context.AnimatorComponent.SetTrigger("charge-end");
            if (Context.ChargeBurnParticles != null)
            {
                Context.ChargeBurnParticles.Play();
            }
        }

        public override void UpdateState()
        {
            _dotTrigger.Step(Time.deltaTime);
            if (_dotTrigger.CheckAndReset())
            {
                Context.Character.ReceiveHit(new HitInfo
                {
                    Multipliers = DamageMultipliers.One,
                    DamageStats = Context.Character.DamageStats,
                    Source = HitSource.AsCharacter(Context.Character)
                });
                _dotTrigger.SetIn(Context.DotTickInterval);
            }

            var speedMultiplier = 3.5f;
            var player = Context.PlayerPosition;
            var self = Context.transform.position;
            var direction = player - self;
            direction.y = 0;

            if (direction.magnitude > 1.5)
            {
                direction.Normalize();
                Context.VelocityMovement.Move(speedMultiplier * new Vector3(direction.x, 0, direction.z));

                var health = Context.Character.Health;
                if (health.Value / health.MaxValue < 0.2f)
                {
                    SwitchState<ExplodingHollowExplosion>();
                }
            }
            else
            {
                direction.Normalize();
                Context.Pushable.Push(direction, 5f, 0.3f);
                SwitchState<ExplodingHollowExplosion>();
            }
        }
        
        public override void OnHitReceived(HitInfo info)
        {
        }

        public override void OnStaggered(HitInfo info)
        {
        }
    }

    public class ExplodingHollowCharging : ExplodingHollowBaseState
    {
        private readonly TimedTrigger _prepare = new TimedTrigger();

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();

            _prepare.Reset();
            
            Context.AnimatorComponent.SetTrigger("charge");
            
            _prepare.SetIn(Context.ChargeTime);
        }

        public override void UpdateState()
        {
            _prepare.Step(Time.deltaTime);
            if (!_prepare.CheckAndReset())
            {
                return;
            }
            
            // ≈сли персонаж не переживет 1 тик урона, то сразу взрываемс€
            if (Context.DotWhileRunning >= Context.Character.Health.Value)
            {
                SwitchState<ExplodingHollowExplosion>();
            }
            else
            {
                SwitchState<ExplodingHollowRunning>();
            }
        }
        
        public override void OnHitReceived(HitInfo info)
        {
        }

        public override void OnStaggered(HitInfo info)
        {
        }
    }

    public class ExplodingHollowExplosion : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.VelocityMovement.Stop();

            Context.AnimatorComponent.SetTrigger("explode");
            Context.ExplosionAttack.StartAttack(_ => SwitchState<ExplodingHollowDeath>());
        }

        public override void OnHitReceived(HitInfo info)
        {
        }

        public override void OnStaggered(HitInfo info)
        {
        }

        public override void OnDeath(HitInfo info)
        {
            Context.ExplosionAttack.InterruptAttack();
            base.OnDeath(info);
        }
    }

    public class ExplodingHollowStagger : ExplodingHollowBaseState
    {
        private readonly TimedTrigger _staggered = new TimedTrigger();
        private bool _wasHit;

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();

            Context.AnimatorComponent.SetBool("is-staggered", true);
            _staggered.SetFor(0.5f);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-staggered", false);
        }

        public override void UpdateState()
        {
            if (_staggered.IsFree)
            {
                if (_wasHit)
                {
                    SwitchState<ExplodingHollowCharging>();
                }
                else
                {
                    SwitchState<ExplodingHollowIdle>();
                }
            }

            _staggered.Step(Time.deltaTime);
        }

        public override void OnHitReceived(HitInfo info)
        {
            if (info.Source.Character is IPlayerCharacter)
            {
                _wasHit = true;
            }
        }

        public override void OnStaggered(HitInfo info)
        {
        }
    }

    public class ExplodingHollowDeath : EnemyBaseState<ExplodingHollow>
    {
        public override void EnterState()
        {
            Context.Hurtbox.Disable();
            Context.VelocityMovement.Stop();
            Context.AnimatorComponent.SetTrigger("death");

            // ѕочти не захардкожено
            Context.StartCoroutine(DieIn(0.3f));
        }

        private IEnumerator DieIn(float time)
        {
            yield return new WaitForSeconds(time);
            Object.Destroy(Context.gameObject);
        }
    }

    public class ExplodingHollow : EnemyStateMachine<ExplodingHollow>
    {
        [SerializeField, Min(0)] private float aggroDistance = 5;
        [SerializeField, Min(0)] private float chargeTime = 2;
        [SerializeField, Min(0)] private float dotWhileRunning = 5f;
        [SerializeField, Min(0)] private float dotTickInterval = 0.3f;

        [SerializeField] private MonoAttackHandler explosionMonoAttackHandler;
        [SerializeField] private MonoAttackHandler punchMonoAttack;
        [SerializeField] private ParticleSystem chargeBurnParticles;

        public float AggroDistance => aggroDistance;
        public float ChargeTime => chargeTime;

        public Animator AnimatorComponent { get; private set; }

        public IAttackExecutor ExplosionAttack => explosionMonoAttackHandler.Executor;
        public IAttackExecutor PunchAttack => punchMonoAttack.Executor;

        public float DotWhileRunning => dotWhileRunning;
        public float DotTickInterval => dotTickInterval;

        public ParticleSystem ChargeBurnParticles => chargeBurnParticles;

        protected override EnemyBaseState<ExplodingHollow> StartState()
        {
            var state = new ExplodingHollowIdle();
            state.Init(this, this);
            return state;
        }

        protected override void Awake()
        {
            base.Awake();
            AnimatorComponent = GetComponent<Animator>();

            if (AnimatorComponent == null)
            {
                Debug.LogWarning("Animator not found", this);
            }

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
