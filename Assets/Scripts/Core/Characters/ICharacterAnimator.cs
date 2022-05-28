namespace Core.Characters
{
    public interface ICharacterAnimator
    {
        void StartWalkAnimation();
        void EndWalkAnimation();
        void StartAirboneAnimation();
        void EndAirboneAnimation();
        void StartStaggerAnimation();
        void EndStaggerAnimation();
        void PlayDeathAnimation();
        void SetCustomTrigger(int hash);
        void SetCustomBool(int hash, bool value);
    }
}