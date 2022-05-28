using Core.Characters;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerAnimator : CharacterAnimator, IPlayerAnimator
    {
        private static readonly int IsSaving = Animator.StringToHash("is-saving");
        private static readonly int StrongAttack1 = Animator.StringToHash("strong-attack");
        private static readonly int LightAttack1 = Animator.StringToHash("attack");
        private static readonly int StrongAttack2 = Animator.StringToHash("strong-attack2");
        private static readonly int LightAttack2 = Animator.StringToHash("attack2");
        private static readonly int AirAttack = Animator.StringToHash("air-attack");
        private static readonly int IsHealing = Animator.StringToHash("is-healing");
        private static readonly int IsDashing = Animator.StringToHash("is-dashing");

        public PlayerAnimator(Animator animator) : base(animator)
        {
        }

        public void StartBonfireAnimation() => SetCustomBool(IsSaving, true);

        public void EndBonfireAnimation() => SetCustomBool(IsSaving, false);

        public void PlayFirstGroundLightAttack() => SetCustomTrigger(LightAttack1);
        public void PlaySecondGroundLightAttack() => SetCustomTrigger(LightAttack2);
        public void PlayAirLightAttack() => SetCustomTrigger(AirAttack);
        public void PlayFirstGroundHeavyAttack() => SetCustomTrigger(StrongAttack1);
        public void PlaySecondGroundHeavyAttack() => SetCustomTrigger(StrongAttack2);

        public void StartHealAnimation() => SetCustomBool(IsHealing, true);
        public void EndHealAnimation() => SetCustomBool(IsHealing, false);

        public void StartDashAnimation() => SetCustomBool(IsDashing, true);

        public void EndDashAnimation() => SetCustomBool(IsDashing, false);
    }
}