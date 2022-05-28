using UnityEngine;

namespace Core.Characters
{
    public class CharacterAnimator : ICharacterAnimator
    {
        private readonly Animator _animator;
        private static readonly int IsWalking = Animator.StringToHash("is-walking");
        private static readonly int IsAirbone = Animator.StringToHash("is-airbone");
        private static readonly int IsStaggered = Animator.StringToHash("is-staggered");
        private static readonly int IsDead = Animator.StringToHash("death");

        public CharacterAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void StartWalkAnimation() => SetCustomBool(IsWalking, true);

        public void EndWalkAnimation() => SetCustomBool(IsWalking, false);
        public void StartAirboneAnimation() => SetCustomBool(IsAirbone, true);

        public void EndAirboneAnimation() => SetCustomBool(IsAirbone, false);

        public void StartStaggerAnimation() => SetCustomBool(IsStaggered, true);

        public void EndStaggerAnimation() => SetCustomBool(IsStaggered, false);

        public void PlayDeathAnimation() => SetCustomBool(IsDead, true);

        public void SetCustomTrigger(int hash)
        {
            _animator.SetTrigger(hash);
        }

        public void SetCustomBool(int hash, bool value)
        {
            _animator.SetBool(hash, value);
        }
    }
}