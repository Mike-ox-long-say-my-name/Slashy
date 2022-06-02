using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueStaggered : RogueBaseState
    {
        public override void EnterState()
        {
            Context.Character.Balance.Frozen = true;
            Context.Animator.StartStaggerAnimation();
        }

        public override void OnStaggerEnded()
        {
            var value = Random.value;
            if (value < 0.4f)
            {
                SwitchState<RogueJumpAway>();
            }
            else if (value < 0.65f)
            {
                SwitchState<RoguePursue>();
            }
            else
            {
                SwitchState<RogueRetreat>();
            }
        }

        public override void ExitState()
        {
            Context.Character.Balance.Frozen = false;
            Context.Animator.EndStaggerAnimation();
        }
    }
}