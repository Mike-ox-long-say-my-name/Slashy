using JetBrains.Annotations;
using UnityEngine;

namespace Core.Characters.Mono
{
    [CreateAssetMenu(menuName = "Movement/Movement Config", fileName = "MovementConfig", order = 0)]
    public class MonoVelocityMovementConfig : ScriptableObject
    {
        [UsedImplicitly]
        [field: SerializeField] public float HorizontalSpeed { get; private set; } = 5;
        [UsedImplicitly]
        [field: SerializeField] public float VerticalSpeed { get; private set; } = 5;
        [UsedImplicitly]
        [field: SerializeField] public float MinVelocity { get; private set; } = -20;
        [UsedImplicitly]
        [field: SerializeField] public float MaxVelocity { get; private set; } = 20;
        [UsedImplicitly]
        [field: SerializeField] public float Gravity { get; private set; } = -9.81f;
        [UsedImplicitly]
        [field: SerializeField] public float GroundedGravity { get; private set; } = -0.5f;
        [UsedImplicitly]
        [field: SerializeField] public float AirboneControlFactor { get; private set; } = 0.7f;
    }
}