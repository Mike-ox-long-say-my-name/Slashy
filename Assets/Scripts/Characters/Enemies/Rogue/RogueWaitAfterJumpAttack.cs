using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueWaitAfterJumpAttack : RogueBaseState
    {
        private Timer _waitTimer;

        public override void EnterState()
        {
            var waitTime = Random.Range(0.3f, 1f);
            _waitTimer = Timer.Start(waitTime, OnTimeout);
        }

        public override void UpdateState()
        {
            _waitTimer?.Tick(Time.deltaTime);
        }

        private void OnTimeout()
        {
            var value = Random.value;
            if (value < 0.3f)
            {
                SwitchState<RogueThrust>();
            }
            else if (value < 0.6f)
            {
                SwitchState<RogueRetreat>();
            }
            else if (value < 0.75f)
            {
                SwitchState<RogueJumpAtPlayer>();
            }
            else
            {
                SwitchState<RogueJumpAway>();
            }
        }
    }
}