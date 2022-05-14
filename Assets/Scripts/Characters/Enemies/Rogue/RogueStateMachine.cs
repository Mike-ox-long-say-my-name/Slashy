using Characters.Enemies.States;
using Core;
using Core.Attacking;
using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueBaseState : EnemyBaseState<RogueStateMachine>
    {
        public override void OnStaggered(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<RogueStaggered>();
        }

        public override void OnDeath(HitInfo info)
        {
            Context.AttackExecutorHelper.InterruptAllRunning();
            SwitchState<RogueDeath>();
        }
    }

    public class RogueDeath : EnemyBaseState<RogueStateMachine>
    {
        public override void EnterState()
        {
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
            var aggroDistance = 6f;
            if (Vector3.Distance(position, player) < aggroDistance)
            {
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
            SwitchState<RoguePursue>();
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-staggered", false);
        }
    }

    public class RoguePursue : RogueBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetBool("is-walking", true);

            var player = Context.PlayerInfo.Transform;
            Context.AutoMovement.MoveTo(player);
            Context.AutoMovement.LockRotationOn(player);
            Context.AutoMovement.SetTargetReachedEpsilon(1);
            Context.AutoMovement.TargetReached += OnTargetReached;
        }

        private void OnTargetReached()
        {
            // TODO: рандомная атака
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-walking", false);
            Context.AutoMovement.ResetState();
            Context.AutoMovement.UnlockRotation();
        }
    }

    public class RogueStateMachine : EnemyStateMachine<RogueStateMachine>
    {
        public MixinAttackExecutorHelper AttackExecutorHelper { get; private set; }
        public IAutoMovement AutoMovement { get; private set; }

        protected override EnemyBaseState<RogueStateMachine> StartState()
        {
            var state = new RogueIdle();
            state.Init(this, this);
            return state;
        }


    }
}