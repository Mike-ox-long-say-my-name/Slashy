using System;
using UnityEngine;

namespace Core.Attacking
{
    [Serializable]
    public struct DamageMultipliers
    {
        [field: SerializeField, Min(0)] public float DamageMultiplier { get; set; }
        [field: SerializeField, Min(0)] public float BalanceDamageMultiplier { get; set; }
        [field: SerializeField, Min(0)] public float PushMultiplier { get; set; }
        [field: SerializeField, Min(0)] public float StaggerTimeMultiplier { get; set; }

        public static DamageMultipliers One = new DamageMultipliers
        {
            BalanceDamageMultiplier = 1,
            DamageMultiplier = 1,
            PushMultiplier = 1,
            StaggerTimeMultiplier = 1
        };
    }
}