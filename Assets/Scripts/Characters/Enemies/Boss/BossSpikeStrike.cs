using Core.Attacking;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossSpikeStrike : BossBaseState
    {
        private static int _spikeStrikesInRow = 0;

        public override void EnterState()
        {
            ++_spikeStrikesInRow;
            Context.Animator.PlaySpikeStrikeAnimation();
            Context.SpikeStrikeExecutor.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasInterrupted)
            {
                return;
            }

            if (_spikeStrikesInRow < Context.MaxSpikeStrikesInRow && Random.value < Context.SpikeStrikeRepeatChance)
            {
                SwitchState<BossSpikeStrike>();
            }
            else
            {
                _spikeStrikesInRow = 0;
                SwitchState<BossWaitAfterAttack>();
            }
        }
    }
}