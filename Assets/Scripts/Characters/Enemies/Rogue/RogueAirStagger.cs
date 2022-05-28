using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueAirStagger : RogueBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            Context.AutoMovement.UnlockRotation();
            Context.Animator.StartStaggerAnimation();
            const float airStaggerTime = 3f;
            _timer = Timer.Start(airStaggerTime, () => SwitchState<RoguePursue>());
        }

        public override void UpdateState()
        {
            _timer.Tick(Time.deltaTime);
        }

        public override void ExitState()
        {
            Context.Animator.EndStaggerAnimation();
        }
    }
}