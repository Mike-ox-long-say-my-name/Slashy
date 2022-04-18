using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public struct DamageInfo
    {
        [SerializeField] private float damage;
        [SerializeField] private float balanceDamage;

        public DamageInfo(float damage, float balanceDamage = 0)
        {
            this.damage = damage;
            this.balanceDamage = balanceDamage;
        }

        public float Damage => damage;
        public float BalanceDamage => balanceDamage;
    }
}