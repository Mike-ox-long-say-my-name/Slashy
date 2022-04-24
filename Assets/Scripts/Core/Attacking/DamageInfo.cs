using System;
using UnityEngine;

namespace Core.Attacking
{
    [Serializable]
    public struct DamageInfo
    {
        [Min(0)] public float damage;
        [Min(0)] public float balanceDamage;
        [Min(0)] public float pushStrength;
        [Min(0)] public float staggerTime;
    }
}