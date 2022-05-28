using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueStaggered : RogueBaseState
    {
        public override void EnterState()
        {
            Context.Animator.StartStaggerAnimation();
        }

        public override void OnStaggerEnded()
        {
            var value = Random.value;
            if (value < 0.2f)
            {
                SwitchState<RogueThrust>();
            }
            else if (value < 0.5f)
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
            Context.Animator.EndStaggerAnimation();
        }
    }
}