using Core.Attacking;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueThrust : RogueBaseState
    {
        public override void EnterState()
        {
            Context.Animator.PlayThrustAnimation();
            Context.ThrustExecutor.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (Random.value < Context.JumpAwayAfterThrustChance)
            {
                SwitchState<RogueJumpAway>();
            }
            else
            {
                SwitchState<RoguePursue>();
            }
        }
    }
}