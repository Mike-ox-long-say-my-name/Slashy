using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossPursue : BossBaseState
    {
        private float _pursueTime;
        private float _maxPursueTime;

        public override void EnterState()
        {
            Context.Animator.StartWalkAnimation();

            var player = Context.Player.Transform;
            Context.AutoMovement.TargetReached += OnTargetReached;
            Context.AutoMovement.MoveTo(player);
            Context.AutoMovement.LockRotationOn(player);
            Context.AutoMovement.SetTargetReachedEpsilon(2.5f);
            Context.AutoMovement.SetSpeedMultiplier(Random.Range(1f, 1.8f));

            _pursueTime = 0;
            _maxPursueTime = Random.Range(2f, 4f);
        }

        public override void UpdateState()
        {
            _pursueTime += Time.deltaTime;

            if (_pursueTime <= _maxPursueTime)
            {
                return;
            }

            var value = Random.value;
            if (value < 0.2f)
            {
                SwitchState<BossJumpAttackStart>();
            }
            else if (value < 0.7f && CanReachPlayerWithDashAttack())
            {
                SwitchState<BossThrustWithDash>();
            }
            else
            {
                SwitchState<BossPrepareSpikeStrike>();
            }
        }

        private bool CanReachPlayerWithDashAttack()
        {
            return Context.MaxDashDistance >= Vector3.Distance(Context.transform.position, Context.PlayerPosition);
        }

        private void OnTargetReached()
        {
            SwitchState<BossWaitBeforeAttack>();
        }

        public override void ExitState()
        {
            Context.Animator.EndWalkAnimation();

            Context.VelocityMovement.Stop();
            Context.AutoMovement.ResetState();
            Context.AutoMovement.UnlockRotation();
        }
    }
}