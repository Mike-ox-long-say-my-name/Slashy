using Attacks;
using Characters.Enemies.States;
using Characters.Player;
using Core.Attacking;
using Core.Utilities;
using System.Collections;
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
            if (direction.magnitude > 1.5f)
            {
                direction.Normalize();
                Context.Movement.Move(new Vector3(direction.x, 0, direction.z));
            }
            else
            {
                Context.Movement.Rotate(direction.x);
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
            if (info.Source.Character is MonoPlayerCharacter)
            {
                SwitchState<ExplodingHollowCharge>();
            }
        }

        public override void OnStaggered(HitInfo info)
        {
            SwitchState<ExplodingHollowStagger>();
        }
    }

    public class ExplodingHollowAttack : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.Movement.Stop();

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
            Context.PunchAttack.InterruptAttack();
            base.OnDeath(info);
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.PunchAttack.InterruptAttack();
            base.OnStaggered(info);
        }

        public override void OnHitReceived(HitInfo info)
        {
            if (info.Source.Character is IPlayerCharacter)
            {
                Context.PunchAttack.InterruptAttack();
            }
            base.OnHitReceived(info);
        }
    }

    public class ExplodingHollowCharge : ExplodingHollowBaseState
    {
        private readonly TimedTriggerFactory _triggerFactory = new TimedTriggerFactory();
        private TimedTrigger _prepare;
        private TimedTrigger _dot;
        private bool _started = false;

        public override void EnterState()
        {
            Context.Movement.Stop();

            _prepare = _triggerFactory.Create();
            _dot = _triggerFactory.Create();

            _prepare.SetFor(Context.ChargeTime);
            _dot.Set();
        }

        public override void UpdateState()
        {
            _triggerFactory.StepAll();

            if (_prepare.IsSet)
            {
                return;
            }

            if (!_started)
            {
                Context.AnimatorComponent.SetTrigger("charge");
                if (Context.ChargeBurnParticles != null)
                {
                    Context.ChargeBurnParticles.Play();
                }
                _started = true;
            }


            if (_dot.CheckAndReset())
            {
                Context.Character.ReceiveHit(new HitInfo
                {
                    DamageInfo = new DamageInfo
                    {
                        damage = Context.DotWhileCharging
                    }
                });
                _dot.SetIn(Context.DotTickInterval);
            }

            var speedMultiplier = 3.5f;
            var player = Context.PlayerPosition;
            var self = Context.transform.position;
            var direction = player - self;
            direction.y = 0;

            if (direction.magnitude > 1.5)
            {
                direction.Normalize();
                Context.Movement.Move(speedMultiplier * new Vector3(direction.x, 0, direction.z));

                var health = Context.Character.Health;
                if (health.Value / health.MaxValue < 0.2f)
                {
                    SwitchState<ExplodingHollowExplosion>();
                }
            }
            else
            {
                direction.Normalize();
                Context.Pushable.Push(direction, 1.4f, 0.3f);
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

    public class ExplodingHollowExplosion : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.Movement.Stop();

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
            Context.Movement.Stop();

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
                    SwitchState<ExplodingHollowCharge>();
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

    public class ExplodingHollowDeath : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.Hurtbox.Disable();
            Context.Movement.Stop();

            Context.StartCoroutine(DieIn(1));
        }

        private IEnumerator DieIn(float time)
        {
            yield return new WaitForSeconds(time);
            Object.Destroy(Context.gameObject);
        }

        public override void OnHitReceived(HitInfo info)
        {
        }

        public override void OnStaggered(HitInfo info)
        {
        }

        public override void OnDeath(HitInfo info)
        {
        }
    }

    public class ExplodingHollow : EnemyStateMachine<ExplodingHollow>
    {
        [SerializeField, Min(0)] private float aggroDistance = 5;
        [SerializeField, Min(0)] private float chargeTime = 2;
        [SerializeField, Min(0)] private float dotWhileCharging = 5f;
        [SerializeField, Min(0)] private float dotTickInterval = 0.3f;

        [SerializeField] private MonoAttackHandler explosionMonoAttackHandler;
        [SerializeField] private MonoAttackHandler punchMonoAttack;
        [SerializeField] private ParticleSystem chargeBurnParticles;

        public float AggroDistance => aggroDistance;
        public float ChargeTime => chargeTime;

        public Animator AnimatorComponent { get; private set; }

        public IAttackExecutor ExplosionAttack => explosionMonoAttackHandler.Executor;
        public IAttackExecutor PunchAttack => punchMonoAttack.Executor;

        public float DotWhileCharging => dotWhileCharging;
        public float DotTickInterval => dotTickInterval;

        public ParticleSystem ChargeBurnParticles => chargeBurnParticles;

        protected override EnemyBaseState<ExplodingHollow> StartState()
        {
            var state = new ExplodingHollowIdle();
            state.Init(this, this);
            return state;
        }

        private void Awake()
        {
            AnimatorComponent = GetComponent<Animator>();

            if (AnimatorComponent == null)
            {
                Debug.LogWarning("Animator not found", this);
            }

            if (punchMonoAttack == null)
            {
                Debug.LogWarning("Explosion Attack Executor is not assigned", this);
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
