using System.Collections;
using Characters.Enemies.States;
using Core.Attacking;
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
            Context.LastHitInfo = info;
            SwitchState<WeakHollowStagger>();
        }

        public override void OnDeath(HitInfo info)
        {
            Context.LastHitInfo = info;
            SwitchState<WeakHollowDeath>();
        }
    }

    public class WeakHollowStagger : WeakHollowBaseState
    {
        private Coroutine _staggerRoutine;

        public override void EnterState()
        {
            if (_staggerRoutine != null)
            {
                Context.StopCoroutine(_staggerRoutine);
            }

            _staggerRoutine = Context.StartCoroutine(StaggerRoutine(Context.LastHitInfo.StaggerTime));
        }

        private IEnumerator StaggerRoutine(float staggerTime)
        {
            yield return new WaitForSeconds(staggerTime);
            SwitchState<WeakHollowIdle>();
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
        }

        public override void UpdateState()
        {
            var playerDirection = Context.PlayerPosition.WithZeroY() - Context.Movement.Transform.position.WithZeroY();
            var distance = playerDirection.magnitude;
            playerDirection.Normalize();
            if (distance < 3)
            {
                SwitchState<WeakHollowCharge>();
            }
            else
            {
                Context.VelocityMovement.Move(playerDirection);
            }
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-walking", false);
        }
    }

    public class WeakHollowCharge : WeakHollowBaseState
    {
        public override void EnterState()
        {
            //Context.AnimatorComponent.Set("is-walking", false);
        }
    }

    public class WeakHollowDeath : WeakHollowBaseState
    {
        public override void EnterState()
        {
            Context.AnimatorComponent.SetTrigger("death");

            Context.StartCoroutine(DieIn(1));
        }

        private IEnumerator DieIn(float time)
        {
            yield return new WaitForSeconds(time);
            Object.Destroy(Context.gameObject);
        }
    }

    public class WeakHollow : EnemyStateMachine<WeakHollow>
    {
        public HitInfo LastHitInfo { get; set; }
        public Animator AnimatorComponent { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            AnimatorComponent = GetComponent<Animator>();
        }

        protected override EnemyBaseState<WeakHollow> StartState()
        {
            var state = new WeakHollowIdle();
            state.Init(this, this);
            return state;
        }
    }
}