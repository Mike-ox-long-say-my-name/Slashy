using Core;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueWait : RogueBaseState
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
            if (Random.value < 0.8f
                && Vector3.Distance(Context.PlayerPosition.WithZeroY(), Context.transform.position.WithZeroY()) > 1)
            {
                SwitchState<RogueJumpAtPlayer>();
            }
            else
            {
                SwitchState<RoguePursue>();
            }
        }
    }
}