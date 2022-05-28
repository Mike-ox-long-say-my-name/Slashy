using Core;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowIdle : WeakHollowBaseState
    {
        public override void UpdateState()
        {
            var distance = Vector3.Distance(Context.PlayerPosition.WithZeroY(),
                Context.transform.position.WithZeroY());
            if (distance > 6)
            {
                return;
            }
                
            Context.Aggro();
            SwitchState<WeakHollowPursue>();
        }
    }
}