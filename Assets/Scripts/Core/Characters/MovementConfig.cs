using System;
using UnityEngine;

namespace Core.Characters
{
    [Serializable]
    public struct MovementConfig
    {
        [field: SerializeField] public float HorizontalSpeed { get; set; }
        [field: SerializeField] public float VerticalSpeed { get; set; }
        [field: SerializeField] public float MinVelocity { get; set; }
        [field: SerializeField] public float MaxVelocity { get; set; }
        [field: SerializeField] public float Gravity { get; set; }
        [field: SerializeField] public float GroundedGravity { get; set; }
    }
}