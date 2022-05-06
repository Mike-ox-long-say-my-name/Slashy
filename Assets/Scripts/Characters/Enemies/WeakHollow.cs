using System.Collections;
using Characters.Enemies.States;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies
{
    public class WeakHollowBaseState : EnemyBaseState<WeakHollow>
    {
        public override void OnHitReceived(HitInfo info)
        {
            Context.LastHitInfo = info;
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.InterruptActiveAttack();
            Context.LastHitInfo = info;
            SwitchState<WeakHollowStagger>();
        }

        public override void OnDeath(HitInfo info)
        {
            Context.InterruptActiveAttack();
            Context.LastHitInfo = info;
            SwitchState<WeakHollowDeath>();
        }
    }

    public class WeakHollowStagger : WeakHollowBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-staggered", true);
            _timer = Timer.Start(Context.LastHitInfo.StaggerTime, SwitchState<WeakHollowIdle>);
        }

        public override void UpdateState()
        {
            _timer.Step(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-staggered", false);
        }
    }

    public class WeakHollowIdle : WeakHollowBaseState
    {
        public override void UpdateState()
        {
            var distance = Vector3.Distance(Context.PlayerPosition.WithZeroY(),
                Context.Movement.Transform.position.WithZeroY());
            if (distance < 6)
            {
                SwitchState<WeakHollowPursue>();
            }
        }
    }

    public class WeakHollowPursue : WeakHollowBaseState
    {
        public override void EnterState()
        { 
            Context.AnimatorComponent.SetBool("is-walking", true);
            Context.AutoMovement.MoveTo(Context.Player.PlayerMovement.BaseMovement.Transform);
            Context.AutoMovement.SetTargetReachedEpsilon(3);
            Context.AutoMovement.TargetReached += SwitchState<WeakHollowCharge>;
        }

        public override void ExitState()
        {
            Context.AutoMovement.ResetState();
            Context.AnimatorComponent.SetBool("is-walking", false);
        }
    }

    public class WeakHollowCharge : WeakHollowBaseState
    {
        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-charging", true);
            _timer = Timer.Start(3, SwitchState<WeakHollowConfused>);

            Context.AutoMovement.SetSpeedMultiplier(2);
            Context.AutoMovement.MoveTo(Context.Player.PlayerMovement.BaseMovement.Transform);
        }

        private Timer _timer;

        public override void UpdateState()
        {

            var playerDirection = Context.PlayerPosition.WithZeroY() - Context.Movement.Transform.position.WithZeroY();
            var distance = playerDirection.magnitude;
            playerDirection.Normalize();
            if (distance < 1)
            {
                SwitchState<WeakHollowAttack>();
                return;
            }

            Context.VelocityMovement.Move(playerDirection * 2);
            _timer.Step(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-charging", false);
            Context.AutoMovement.ResetState();
        }
    }

    public class WeakHollowConfused : WeakHollowBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            _timer = Timer.Start(3, SwitchState<WeakHollowPursue>);
        }

        public override void UpdateState()
        {
            _timer.Step(Time.deltaTime);
        }
    }

    public class WeakHollowAttack : WeakHollowBaseState
    {
        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            Context.AnimatorComponent.SetTrigger("attack");
            Context.PunchAttackExecutor.StartAttack(result =>
            {
                if (result.WasInterrupted)
                {
                    return;
                }

                if (result.Hits.Count > 0)
                {
                    SwitchState<WeakHollowRetreat>();
                }
                else
                {
                    SwitchState<WeakHollowPursue>();
                }
            });
        }
    }

    public class WeakHollowDeath : WeakHollowBaseState
    {
        public override void EnterState()
        {
            Context.AnimatorComponent.SetTrigger("death");

            Context.StartCoroutine(DieIn(0.5f));
        }

        private IEnumerator DieIn(float time)
        {
            yield return new WaitForSeconds(time);
            Object.Destroy(Context.gameObject);
        }
    }

    public class WeakHollowRetreat : WeakHollowBaseState
    {
        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-walking", true);

            Context.AutoMovement.SetSpeedMultiplier(0.4f);
            Context.AutoMovement.LockRotationOn(Context.Player.PlayerMovement.BaseMovement.Transform);

            _timer = Timer.Start(2, SwitchState<WeakHollowIdle>);
        }

        private Timer _timer;

        public override void UpdateState()
        {
            var playerDirection = Context.PlayerPosition.WithZeroY() - Context.Movement.Transform.position.WithZeroY();
            playerDirection.Normalize();
            
            Context.AutoMovement.Move(-playerDirection);

            _timer.Step(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.AutoMovement.ResetSpeedMultiplier();
            Context.AutoMovement.UnlockRotation();
            Context.AnimatorComponent.SetBool("is-walking", false);
        }
    }

    public class WeakHollow : EnemyStateMachine<WeakHollow>
    {
        [SerializeField] private MonoAttackHandler punchAttack;

        public HitInfo LastHitInfo { get; set; }
        public Animator AnimatorComponent { get; private set; }

        public IAttackExecutor PunchAttackExecutor => punchAttack.Executor;
        public IAutoMovement AutoMovement { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            AnimatorComponent = GetComponent<Animator>();
            AutoMovement = VelocityMovement as IAutoMovement;
        }

        protected override EnemyBaseState<WeakHollow> StartState()
        {
            var state = new WeakHollowIdle();
            state.Init(this, this);
            return state;
        }

        public void InterruptActiveAttack()
        {
            if (PunchAttackExecutor.IsAttacking)
            {
                PunchAttackExecutor.InterruptAttack();
            }
        }
    }
}