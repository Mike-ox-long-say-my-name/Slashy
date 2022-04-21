using Attacking;
using Characters.Enemies.States;
using Core.Characters;
using Core.Utilities;
using System;
using System.Collections;
using Characters.Player;
using Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters.Enemies
{
    public class ExplodingHollowIdle : ExplodingHollowBaseState
    {
        public override void UpdateState()
        {
            Context.Movement.ApplyGravity();
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
            Context.Movement.ApplyGravity();

            var player = Context.PlayerPosition;
            var self = Context.transform.position;
            var direction = player - self;
            direction.y = 0;
            if (direction.magnitude > 1.5f)
            {
                direction.Normalize();
                Context.Movement.Move(new Vector2(direction.x, direction.z));
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
        public override void InterruptState(CharacterInterruption interruption)
        {
            switch (interruption.Type)
            {
                case CharacterInterruptionType.Death:
                    SwitchState<ExplodingHollowDeath>();
                    break;
                case CharacterInterruptionType.Hit:
                    if (interruption.Source is PlayerCharacter)
                    {
                        SwitchState<ExplodingHollowCharge>();
                    }
                    break;
                case CharacterInterruptionType.Staggered:
                    SwitchState<ExplodingHollowStagger>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interruption), interruption, null);
            }
        }
    }

    public class ExplodingHollowAttack : ExplodingHollowBaseState
    {
        public override void EnterState()
        {
            Context.Movement.ResetXZVelocity();
            Context.PunchAttack.StartExecution(Context.Character, inter =>
            {
                if (!inter)
                {
                    SwitchState<ExplodingHollowPursue>();
                }
            });
        }

        public override void InterruptState(CharacterInterruption interruption)
        {
            Context.PunchAttack.InterruptAttack();
            base.InterruptState(interruption);
        }
    }

    public class ExplodingHollowCharge : ExplodingHollowBaseState
    {
        private readonly TimedTriggerFactory _triggerFactory = new TimedTriggerFactory();
        private TimedTrigger _prepare;
        private TimedTrigger _dot;

        public override void EnterState()
        {
            _prepare = _triggerFactory.Create();
            _dot = _triggerFactory.Create();

            Context.Movement.ResetXZVelocity();
            _prepare.SetFor(Context.ChargeTime);
            _dot.Set();
        }

        public override void UpdateState()
        {
            Context.Movement.ApplyGravity();

            _triggerFactory.StepAll();

            if (_prepare.IsSet)
            {
                return;
            }
            
            // TODO: cringe
            Context.AnimatorComponent.SetTrigger("charge");

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
                Context.Movement.Move(speedMultiplier * new Vector2(direction.x, direction.z));

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

        public override void InterruptState(CharacterInterruption interruption)
        {
            if (interruption.Type == CharacterInterruptionType.Death)
            {
                SwitchState<ExplodingHollowDeath>();
            }
        }
    }

    public class ExplodingHollowExplosion : ExplodingHollowBaseState
    {
        public override void InterruptState(CharacterInterruption interruption)
        {
            if (interruption.Type == CharacterInterruptionType.Death)
            {
                Context.ExplosionAttackExecutor.InterruptAttack();
            }
            base.InterruptState(interruption);
        }

        public override void EnterState()
        {
            Context.AnimatorComponent.SetTrigger("explode");
            Context.Movement.ResetXZVelocity();
            Context.ExplosionAttackExecutor.StartExecution(Context.Character,
                _ => SwitchState<ExplodingHollowDeath>());
        }
    }

    public class ExplodingHollowStagger : ExplodingHollowBaseState
    {
        private readonly TimedTrigger _staggered = new TimedTrigger();

        public override void EnterState()
        {
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
                SwitchState<ExplodingHollowPursue>();
            }
            _staggered.Step(Time.deltaTime);
        }

        public override void InterruptState(CharacterInterruption interruption)
        {
            if (interruption.Type != CharacterInterruptionType.Hit)
            {
                base.InterruptState(interruption);
            }
        }
    }

    public class ExplodingHollowDeath : ExplodingHollowBaseState
    {
        public override void InterruptState(CharacterInterruption interruption)
        {
        }

        public override void EnterState()
        {
            Context.Movement.ResetXZVelocity();
            Context.StartCoroutine(DieIn(1));
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
        [SerializeField, Min(0)] private float dotWhileCharging = 5f;
        [SerializeField, Min(0)] private float dotTickInterval = 0.3f;

        [SerializeField] private Pushable pushable;
        [SerializeField] private Animator animatorComponent;
        [SerializeField] private AttackExecutor punchAttack;
        [SerializeField] private AttackExecutor explosionAttackExecutor;

        public float AggroDistance => aggroDistance;
        public float ChargeTime => chargeTime;

        public Pushable Pushable => pushable;
        public Animator AnimatorComponent => animatorComponent;
        public AttackExecutor ExplosionAttackExecutor => explosionAttackExecutor;
        public AttackExecutor PunchAttack => punchAttack;

        public float DotWhileCharging => dotWhileCharging;
        public float DotTickInterval => dotTickInterval;

        protected override EnemyBaseState<ExplodingHollow> StartState()
        {
            var state = new ExplodingHollowIdle();
            state.Init(this, this);
            return state;
        }

        protected override void Awake()
        {
            if (pushable == null)
            {
                Debug.LogWarning("Pushable is not assigned", this);
                enabled = false;
            }
            if (animatorComponent == null)
            {
                Debug.LogWarning("Animator is not assigned", this);
                enabled = false;
            }
            if (punchAttack == null)
            {
                Debug.LogWarning("Explosion Attack Executor is not assigned", this);
                enabled = false;
            }
            if (explosionAttackExecutor == null)
            {
                Debug.LogWarning("Explosion Attack Executor is not assigned", this);
                enabled = false;
            }

            base.Awake();
        }
    }
}
