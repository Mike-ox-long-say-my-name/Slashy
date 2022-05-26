using UnityEngine;

namespace Core.Characters
{
    public interface ICharacterAnimator
    {
        void StartWalkAnimation();
        void StopWalkAnimation();
        void StartHurtAnimation();
        void StopHurtAnimation();
        void PlayDeathAnimation();
    }

    public class CharacterAnimator : ICharacterAnimator
    {
        private readonly Animator _baseAnimator;

        public CharacterAnimator(Animator baseAnimator)
        {
            _baseAnimator = baseAnimator;
        }

        public void StartWalkAnimation()
        {
            _baseAnimator.SetBool("is-walking", true);
        }

        public void StopWalkAnimation()
        {
            _baseAnimator.SetBool("is-walking", false);
        }

        public void StartHurtAnimation()
        {
            _baseAnimator.SetBool("is-staggered", true);
        }

        public void StopHurtAnimation()
        {
            _baseAnimator.SetBool("is-staggered", false);
        }

        public void PlayDeathAnimation()
        {
            _baseAnimator.SetTrigger("death");
        }
    }
}