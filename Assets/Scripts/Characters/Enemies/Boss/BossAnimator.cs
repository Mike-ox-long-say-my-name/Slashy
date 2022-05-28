using Core.Characters;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossAnimator : CharacterAnimator
    {
        private static readonly int Thrust = Animator.StringToHash("thrust");
        private static readonly int SpikeStrike = Animator.StringToHash("spike-strike");
        private static readonly int PrepareSpikeStrike = Animator.StringToHash("prepare-spike-strike");
        private static readonly int JumpStart = Animator.StringToHash("jump-start");
        private static readonly int JumpEnd = Animator.StringToHash("jump-end");
        private static readonly int Swing = Animator.StringToHash("horizontal-swing");

        public BossAnimator(Animator animator) : base(animator)
        {
        }

        public void PlayThrustAnimation() => SetCustomTrigger(Thrust);
        public void PlaySwingAnimation() => SetCustomTrigger(Swing);

        public void PlaySpikeStrikeAnimation() => SetCustomTrigger(SpikeStrike);

        public void PlayPrepareSpikeStrikeAnimation() => SetCustomTrigger(PrepareSpikeStrike);

        public void PlayJumpStartAnimation() => SetCustomTrigger(JumpStart);

        public void PlayJumpEndAnimation() => SetCustomTrigger(JumpEnd);
    }
}