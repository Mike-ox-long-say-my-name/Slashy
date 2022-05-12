using Core;
using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowRetreat : WeakHollowBaseState
    {
        private void SetRandomSpeedMultiplier()
        {
            var multiplier = Random.Range(0.3f, 0.7f);
            Context.AutoMovement.SetSpeedMultiplier(multiplier);
        }

        private static float GetRandomRetreatTime()
        {
            return Random.Range(1f, 3f);
        }

        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-walking", true);

            Context.AutoMovement.LockRotationOn(Context.PlayerInfo.Transform);

            SetRandomSpeedMultiplier();

            var retreatTime = GetRandomRetreatTime();
            _timer = Timer.Start(retreatTime, SwitchState<WeakHollowPursue>);
        }

        private Timer _timer;

        public override void UpdateState()
        {
            var playerDirection = (Context.PlayerPosition - Context.BaseMovement.Transform.position).WithZeroY();
            playerDirection.Normalize();
            
            Context.VelocityMovement.Move(-playerDirection);

            _timer.Tick(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.AutoMovement.ResetSpeedMultiplier();
            Context.AutoMovement.UnlockRotation();
            Context.VelocityMovement.Stop();
            Context.AnimatorComponent.SetBool("is-walking", false);
        }
    }
}