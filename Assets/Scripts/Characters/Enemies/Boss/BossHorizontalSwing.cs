using Core.Attacking;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossHorizontalSwing : BossBaseState
    {
        public override void EnterState()
        {
            Context.Animator.PlaySwingAnimation();
            Context.HorizontalSwingExecutor.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasInterrupted)
            {
                return;
            }

            if (Random.value < Context.ThrustAfterSwingChance)
            {
                SwitchState<BossThrustWithDash>();
            }
            else
            {
                SwitchState<BossWaitAfterAttack>();
            }
        }
    }
}