using Core;
using Core.Characters;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowCharge : WeakHollowBaseState
    {
        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-charging", true);

            var time = Random.Range(2.5f, 4.5f);
            _timer = Timer.Start(time, () => SwitchState<WeakHollowConfused>());

            Context.AutoMovement.SetSpeedMultiplier(2);
            Context.AutoMovement.MoveTo(Context.PlayerInfo.Transform);
        }

        private Timer _timer;

        public override void UpdateState()
        {
            var playerDirection = (Context.PlayerPosition - Context.BaseMovement.Transform.position).WithZeroY();
            var distance = playerDirection.magnitude;
            playerDirection.Normalize();
            if (distance < 1)
            {
                SwitchState<WeakHollowAttack>();
                return;
            }

            Context.VelocityMovement.Move(playerDirection * 2);
            _timer.Tick(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-charging", false);
            Context.AutoMovement.ResetState();
        }
    }
}