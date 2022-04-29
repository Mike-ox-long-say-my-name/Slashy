using System;
using UnityEngine;

namespace Core.Characters
{
    [Serializable]
    public struct DamageStats
    {
        [field: SerializeField] public float BaseDamage { get; set; }
        [field: SerializeField] public float BaseBalanceDamage { get; set; }
        [field: SerializeField] public float BaseStaggerTime { get; set; }
        [field: SerializeField] public float BasePushTime { get; set; }
        [field: SerializeField] public float BasePushForce { get; set; }
    }
}