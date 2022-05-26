using Core;
using UnityEngine;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowIdle : ExplodingHollowBaseState
    {
        public override void UpdateState()
        {
            if (Vector3.Distance(Context.PlayerPosition, Context.transform.position) < Context.AggroDistance)
            {
                AggroListener.Instance.IncreaseAggroCounter();
                SwitchState<ExplodingHollowPursue>();
            }
        }
    }
}