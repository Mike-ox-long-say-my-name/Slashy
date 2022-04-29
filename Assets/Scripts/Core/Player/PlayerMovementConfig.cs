using System;
using UnityEngine;

namespace Core.Player
{
    [Serializable]
    public struct PlayerMovementConfig
    {
        [field: SerializeField] public float JumpStartVelocity { get; set; }
        [field: SerializeField] public float AirboneControlFactor { get; set; }
    }
}