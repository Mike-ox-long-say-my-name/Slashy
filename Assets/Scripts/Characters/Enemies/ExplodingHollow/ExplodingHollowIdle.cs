using Core;
using UnityEngine;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowIdle : ExplodingHollowBaseState
    {
        public override void UpdateState()
        {
            var distance = Vector3.Distance(Context.PlayerPosition.WithZeroY(),
                Context.transform.position.WithZeroY());
            if (distance > Context.AggroDistance)
            {
                return;
            }

            Context.AggroModule.Aggro();
        }

        public override void OnAggroed()
        {
            SwitchState<ExplodingHollowPursue>();
        }
    }
}