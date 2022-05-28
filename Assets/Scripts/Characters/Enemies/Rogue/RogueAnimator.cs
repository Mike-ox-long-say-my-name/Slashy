using Core.Characters;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueAnimator : CharacterAnimator
    {
        private static readonly int IsJumpingAway = Animator.StringToHash("is-jumping-away");
        private static readonly int IsJumpingAt = Animator.StringToHash("is-jumping");
        private static readonly int JumpAttack = Animator.StringToHash("jump-attack");
        private static readonly int TripleSlash = Animator.StringToHash("triple-slash");
        private static readonly int Thrust = Animator.StringToHash("thrust");

        public RogueAnimator(Animator animator) : base(animator)
        {
        }

        public void StartJumpAwayAnimation() => SetCustomBool(IsJumpingAway, true);
        public void EndJumpAwayAnimation() => SetCustomBool(IsJumpingAway, false);
        public void StartJumpAtAnimation() => SetCustomBool(IsJumpingAt, true);
        public void EndJumpAtAnimation() => SetCustomBool(IsJumpingAt, false);
        public void PlayJumpAttackAnimation() => SetCustomTrigger(JumpAttack);
        public void PlayTripleSlashAnimation() => SetCustomTrigger(TripleSlash);
        public void PlayThrustAnimation() => SetCustomTrigger(Thrust);
    }
}