using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueAirStagger : RogueBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            Context.Character.Balance.Frozen = true;
            Context.AutoMovement.UnlockRotation();
            Context.Animator.StartStaggerAnimation();
            const float airStaggerTime = 1.5f;
            _timer = Timer.Start(airStaggerTime, () => SwitchState<RoguePursue>());
        }

        public override void UpdateState()
        {
            _timer.Tick(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.Character.Balance.Frozen = false;
            Context.Animator.EndStaggerAnimation();
        }
    }
}