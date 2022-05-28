using Core.Characters;
using UnityEngine;

namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowAnimator : CharacterAnimator
    {
        private static readonly int Punch = Animator.StringToHash("attack");
        private static readonly int IsCharging = Animator.StringToHash("is-charging");

        public WeakHollowAnimator(Animator animator) : base(animator)
        {
        }

        public void PlayPunchAnimation() => SetCustomTrigger(Punch);

        public void StartChargeAnimation() => SetCustomBool(IsCharging, true);

        public void EndChargeAnimation() => SetCustomBool(IsCharging, false);
    }
}