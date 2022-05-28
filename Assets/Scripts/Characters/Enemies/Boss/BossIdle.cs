using Core;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossIdle : BossBaseState
    {
        public override void UpdateState()
        {
            var distance = Vector3.Distance(Context.PlayerPosition.WithZeroY(),
                Context.transform.position.WithZeroY());
            if (distance > 10f)
            {
                return;
            }

            Context.Aggro();
            Context.BossEvents.FightStarted?.Invoke();
            SwitchState<BossPursue>();
        }
    }
}