using UnityEngine;

namespace Characters.Enemies
{
    public class ExplodingHollowIdle : ExplodingHollowBaseState
    {
        public override void UpdateState()
        {
            if (Vector3.Distance(Context.PlayerPosition, Context.transform.position) < Context.AggroDistance)
            {
                SwitchState<ExplodingHollowPursue>();
            }
        }
    }
}