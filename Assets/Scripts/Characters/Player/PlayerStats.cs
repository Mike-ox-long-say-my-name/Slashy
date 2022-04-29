using System;
using UnityEngine;

namespace Characters.Player
{
    [Serializable]
    public struct PlayerStats
    {
        [field: SerializeField] public bool FreezeStamina { get; set; }
        [field: SerializeField] public float MaxStamina { get; set; }
        [field: SerializeField] public float StaminaRegeneration { get; set; }
        [field: SerializeField] public float StaminaRegenerationDelay { get; set; }
        [field: SerializeField] public float EmptyStaminaAdditionalRegenerationDelay { get; set; }
    }
}