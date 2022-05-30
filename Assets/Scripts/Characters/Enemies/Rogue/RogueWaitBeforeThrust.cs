using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueWaitBeforeThrust : RogueBaseState
    {
        private Timer _timer;

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();
            _timer = Timer.Start(Random.Range(0.2f, 0.6f), () => SwitchState<RogueThrust>());
        }

        public override void UpdateState()
        {
            _timer?.Tick(Time.deltaTime);
        }
    }
}