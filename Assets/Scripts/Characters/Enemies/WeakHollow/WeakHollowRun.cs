using Core.Characters;
using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowRun : WeakHollowBaseState
    {
        public override void EnterState()
        {
            Context.Animator.StartChargeAnimation();

            var maxRunTime = Random.Range(2.5f, 4.5f);
            _timer = Timer.Start(maxRunTime, () => SwitchState<WeakHollowConfused>());

            Context.AutoMovement.SetSpeedMultiplier(2);
            Context.AutoMovement.MoveTo(Context.Player.Transform);
            Context.AutoMovement.SetTargetReachedEpsilon(1);
            Context.AutoMovement.TargetReached += () => SwitchState<WeakHollowAttack>();
        }

        private Timer _timer;

        public override void UpdateState()
        {
            _timer?.Tick(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.Animator.EndChargeAnimation();
            Context.VelocityMovement.Stop();
            Context.AutoMovement.ResetState();
        }
    }
}