using Core;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueIdle : RogueBaseState
    {
        public override void UpdateState()
        {
            var player = Context.PlayerPosition.WithZeroY();
            var position = Context.transform.position.WithZeroY();

            var distance = Vector3.Distance(position, player);
            if (distance > Context.AggroModule.AggroDistance)
            {
                return;
            }

            Context.AggroModule.Aggro();
        }

        public override void OnAggroed()
        {
            SwitchState<RoguePursue>();
        }
    }
}