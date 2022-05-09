using Core;
using UnityEngine;

namespace Characters.Enemies
{
    public class WeakHollowIdle : WeakHollowBaseState
    {
        public override void UpdateState()
        {
            var distance = Vector3.Distance(Context.PlayerPosition.WithZeroY(),
                Context.BaseMovement.Transform.position.WithZeroY());
            if (distance < 6)
            {
                SwitchState<WeakHollowPursue>();
            }
        }
    }
}