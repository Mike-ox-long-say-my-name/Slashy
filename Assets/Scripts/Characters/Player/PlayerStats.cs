using Core.Characters;
using UnityEngine;

namespace Characters.Player
{
    [CreateAssetMenu(menuName = "Player Stats", fileName = "PlayerStats", order = 0)]
    public class PlayerStats : CharacterStats, IPlayerStats
    {
        [SerializeField] private bool freezeStamina;
        [SerializeField, Min(0)] private float maxStamina;
        [SerializeField, Min(0)] private float staminaRegeneration;
        [SerializeField, Min(0)] private float staminaRegenerationDelay;
        [SerializeField, Min(0)] private float emptyStaminaAdditionalRegenerationDelay;

        public bool FreezeStamina => freezeStamina;
        public float MaxStamina => maxStamina;
        public float StaminaRegeneration => staminaRegeneration;
        public float StaminaRegenerationDelay => staminaRegenerationDelay;
        public float EmptyStaminaAdditionalRegenerationDelay => emptyStaminaAdditionalRegenerationDelay;
    }
}