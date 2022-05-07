using JetBrains.Annotations;
using UnityEngine;

namespace Core.Characters.Mono
{
    [CreateAssetMenu(menuName = "Movement/Movement Config", fileName = "MovementConfig", order = 0)]
    public class MonoVelocityMovementConfig : ScriptableObject
    {
        [UsedImplicitly]
        [field: SerializeField] public float HorizontalSpeed { get; private set; }
        [UsedImplicitly]
        [field: SerializeField] public float VerticalSpeed { get; private set; }
        [UsedImplicitly]
        [field: SerializeField] public float MinVelocity { get; private set; }
        [UsedImplicitly]
        [field: SerializeField] public float MaxVelocity { get; private set; }
        [UsedImplicitly]
        [field: SerializeField] public float Gravity { get; private set; }
        [UsedImplicitly]
        [field: SerializeField] public float GroundedGravity { get; private set; }
        [UsedImplicitly]
        [field: SerializeField] public float AirboneControlFactor { get; private set; }
    }
}