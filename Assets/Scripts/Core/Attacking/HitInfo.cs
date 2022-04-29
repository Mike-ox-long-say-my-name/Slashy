using Core.Characters;

namespace Core.Attacking
{
    public class HitInfo
    {
        public HitSource Source { get; set; }
        public DamageMultipliers Multipliers { get; set; }
        public DamageStats DamageStats { get; set; }

        public float Damage => Multipliers.DamageMultiplier * DamageStats.BaseDamage;
        public float BalanceDamage => Multipliers.BalanceDamageMultiplier * DamageStats.BaseBalanceDamage;
        public float PushForce => Multipliers.PushMultiplier * DamageStats.BasePushForce;
        public float PushTime => Multipliers.PushMultiplier * DamageStats.BasePushTime;
        public float StaggerTime => Multipliers.StaggerTimeMultiplier * DamageStats.BaseStaggerTime;
    }
}