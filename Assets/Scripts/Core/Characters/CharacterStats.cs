using System;
using UnityEngine;

namespace Core.Characters
{
    [Serializable]
    public struct CharacterStats
    {
        [field: SerializeField] public float MaxHealth { get; set; }
        [field: SerializeField] public float MaxBalance { get; set; }

        [field: SerializeField] public bool FreezeHealth { get; set; }
        [field: SerializeField] public bool FreezeBalance { get; set; }
        [field: SerializeField] public bool CanDie { get; set; }
    }
}