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
        [field: SerializeField, Min(0)] public float EmptyStaminaRegenerationDelay { get; private set; } = 0;
    }
}
