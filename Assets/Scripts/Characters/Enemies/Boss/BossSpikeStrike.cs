using Core.Attacking;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossSpikeStrike : BossBaseState
    {
        public override void EnterState()
        {
            Context.Animator.PlaySpikeStrikeAnimation();
            Context.SpikeStrikeExecutor.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasInterrupted)
            {
                return;
            }

            if (Random.value < Context.SpikeStrikeRepeatChance)
            {
                SwitchState<BossSpikeStrike>();
            }
            else
            {
                SwitchState<BossWaitAfterAttack>();
            }
        }
    }
}