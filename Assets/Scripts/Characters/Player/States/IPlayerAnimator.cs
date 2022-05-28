using Core.Characters;

namespace Characters.Player.States
{
    public interface IPlayerAnimator : ICharacterAnimator
    {
        void StartBonfireAnimation();
        void EndBonfireAnimation();
        void PlayFirstGroundLightAttack();
        void PlaySecondGroundLightAttack();
        void PlayAirLightAttack();
        void PlayFirstGroundHeavyAttack();
        void PlaySecondGroundHeavyAttack();
        void EndDashAnimation();
        void StartDashAnimation();
        void StartHealAnimation();
        void EndHealAnimation();
    }
}