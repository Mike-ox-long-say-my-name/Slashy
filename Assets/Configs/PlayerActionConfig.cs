using UnityEngine;

namespace Configs
{
    [CreateAssetMenu]
    public class PlayerActionConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public float JumpStaminaCost { get; private set; } = 0;
        [field: SerializeField, Min(0)] public float DashStaminaCost { get; private set; } = 0;
        [field: SerializeField, Range(0, 1)] public float DashInvincibilityStart { get; private set; } = 0.1f;
        [field: SerializeField, Range(0, 1)] public float DashInvincibilityEnd { get; private set; } = 0.8f;
        [field: SerializeField, Min(0)] public float StaminaRegeneration { get; private set; } = 0;
        [field: SerializeField, Min(0)] public float StaminaRegenerationDelay { get; private set; } = 0;
        [field: SerializeField, Min(0)] public float EmptyStaminaAdditionalRegenerationDelay { get; private set; } = 0;
        [field: SerializeField, Min(0)] public float LightAttackFirstStaminaCost { get; private set; } = 0;
        [field: SerializeField, Min(0)] public float LightAttackSecondStaminaCost { get; private set; } = 0;
        [field: SerializeField, Min(0)] public float LightAttackRecovery { get; private set; } = 0.1f;
        [field: SerializeField, Min(0)] public float ActiveHealRate { get; private set; } = 10f;
        [field: SerializeField, Min(0)] public float HealStaminaConsumption { get; private set; } = 10f;
        [field: SerializeField, Min(0)] public float StaggerTime { get; private set; } = 0.15f;
        [field: SerializeField, Min(0)] public float StaggerRecoveryTime { get; private set; } = 0.13f;
    }
}
