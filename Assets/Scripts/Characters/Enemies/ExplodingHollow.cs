using Attacking;
using Characters.Enemies.States;
using Core.Characters;
using Core.Utilities;
using System;
using System.Collections;
using Characters.Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters.Enemies
{
    public class ExplodingHollowIdle : ExplodingHollowBaseState
    {
        public override void UpdateState()
        {
            Context.Movement.ApplyGravity();
            if (Vector3.Distance(Context.PlayerPosition, Context.transform.position) < 10)
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
                if (!inter) SwitchState<ExplodingHollowPursue>();
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
        private readonly TimedTrigger _prepare = new TimedTrigger();

        public override void EnterState()
        {
            Context.Movement.ResetXZVelocity();
            _prepare.SetFor(3);
        }

        public override void UpdateState()
        {
            Context.Movement.ApplyGravity();

            if (_prepare.IsSet)
            {
                _prepare.Step(Time.deltaTime);
                return;
            }

            var speedMultiplier = 3.5f;
            var player = Context.PlayerPosition;
            var self = Context.transform.position;
            if (Vector3.Distance(player, self) > 1)
            {
                var direction = player - self;
                direction.y = 0;
                direction.Normalize();

                Context.Movement.Move(speedMultiplier * new Vector2(direction.x, direction.z));
            }
            else
            {
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
                if (Context.HasExplosionAttackExecutor && Context.ExplosionAttackExecutor.IsAttacking)
                {
                    Context.ExplosionAttackExecutor.InterruptAttack();
                }
            }
            base.InterruptState(interruption);
        }

        public override void EnterState()
        {
            Context.AnimatorComponent.SetTrigger("explode");
            Context.Movement.ResetXZVelocity();
            if (Context.HasExplosionAttackExecutor)
            {
                Context.ExplosionAttackExecutor.StartExecution(Context.Character,
                    _ => SwitchState<ExplodingHollowDeath>());
            }
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
            Context.transform.eulerAngles += new Vector3(0, 0, 90);
            Context.transform.position += Vector3.down * 0.5f;
            Context.StartCoroutine(DieIn(2));
        }

        private IEnumerator DieIn(float time)
        {
            yield return new WaitForSeconds(time);
            Object.Destroy(Context.gameObject);
        }
    }

    public class ExplodingHollow : EnemyStateMachine<ExplodingHollow>
    {
        [SerializeField] private Animator animatorComponent;
        [SerializeField] private AttackExecutor punchAttack;
        [SerializeField] private AttackExecutor explosionAttackExecutor;

        public Animator AnimatorComponent => animatorComponent;
        public AttackExecutor ExplosionAttackExecutor => explosionAttackExecutor;
        public AttackExecutor PunchAttack => punchAttack;
        public bool HasExplosionAttackExecutor { get; private set; } = true;

        protected override EnemyBaseState<ExplodingHollow> StartState()
        {
            var state = new ExplodingHollowIdle();
            state.Init(this, this);
            return state;
        }

        protected override void Awake()
        {
            if (explosionAttackExecutor == null)
            {
                Debug.LogWarning("Explosion Attack Executor is not assigned", this);
                HasExplosionAttackExecutor = false;
            }

            base.Awake();
        }
    }
}
