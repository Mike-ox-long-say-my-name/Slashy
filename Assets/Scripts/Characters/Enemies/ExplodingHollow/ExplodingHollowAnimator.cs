using Characters.Enemies.WeakHollow;
using UnityEngine;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowAnimator : WeakHollowAnimator
    {
        private static readonly int Explode = Animator.StringToHash("explode");
        private static readonly int Charge = Animator.StringToHash("charge");
        private static readonly int ChargeEnd = Animator.StringToHash("charge-end");

        public ExplodingHollowAnimator(Animator animator) : base(animator)
        {
        }

        public void PlayExplodeAnimation() => SetCustomTrigger(Explode);

        public void PlayChargeEndAnimation() => SetCustomTrigger(ChargeEnd);

        public void PlayChargeAnimation() => SetCustomTrigger(Charge);
    }
}