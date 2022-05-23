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
            Context.Animator.SetBool("is-walking", true);

            var player = Context.PlayerInfo.Transform;
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
            if (value < 0.4f)
            {
                SwitchState<BossJumpAttackStart>();
            }
            else if (value < 0.7f && Context.MaxDashDistance >= Vector3.Distance(Context.transform.position, Context.PlayerPosition))
            {
                SwitchState<BossThrustWithDash>();
            }
            else
            {
                SwitchState<BossPrepareSpikeStrike>();
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
}