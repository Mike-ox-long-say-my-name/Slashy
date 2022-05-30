using UnityEngine;

namespace Configs.Player
{
    [CreateAssetMenu]
    public class PlayerConfig : ScriptableObject
    {
        [field: Space]
        [field: Header("Attacks")]
        [field: SerializeField, Min(0)] public float LightAttackFirstStaminaCost { get; private set; } = 0;
        [field: SerializeField, Min(0)] public float LightAttackSecondStaminaCost { get; private set; } = 0;
        [field: SerializeField, Min(0)] public float LightAirAttackStaminaCost { get; private set; } = 10;
        [field: SerializeField, Min(0)] public float FirstStrongAttackStaminaCost { get; private set; } = 20;
        [field: SerializeField, Min(0)] public float SecondStrongAttackStaminaCost { get; private set; } = 20;
        [field: SerializeField, Min(0)] public float FirstStrongAttackHealthCost { get; private set; } = 20;
        
        [field: Space]
        [field: Header("Stagger")]
        [field: SerializeField, Min(0)] public float StaggerFallTime { get; private set; } = 0.5f;
        
        [field: Space]
        [field: Header("Jump")]
        [field: SerializeField, Min(0)] public float JumpStaminaCost { get; private set; } = 0;

        [field: Space]
        [field: Header("Dash")]
        [field: SerializeField, Min(0)] public float DashRecovery { get; private set; } = 0.3f;
        [field: SerializeField, Min(0)] public float DashDistance { get; private set; } = 3f;
        [field: SerializeField, Min(0)] public float DashTime { get; private set; } = 0.4f;
        [field: SerializeField, Range(0, 1)] public float DashInvincibilityStart { get; private set; } = 0.1f;
        [field: SerializeField, Range(0, 1)] public float DashInvincibilityEnd { get; private set; } = 0.8f;
        [field: SerializeField, Min(0)] public float DashStaminaCost { get; private set; } = 0;

        [field: Space]
        [field: Header("Heal")]
        [field: SerializeField, Min(0)] public float HealPerTick { get; private set; } = 10f;
        [field: SerializeField, Min(0)] public float HealStaminaConsumptionPerTick { get; private set; } = 10f;
        [field: SerializeField, Min(0)] public float HealTickInterval { get; private set; } = 0.6f;
        [field: SerializeField, Min(0)] public float HealTickPurityCost { get; private set; } = 2.5f;
    }
}
