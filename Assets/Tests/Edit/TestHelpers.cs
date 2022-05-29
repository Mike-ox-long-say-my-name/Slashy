using Core.Attacking;
using Core.Characters;

namespace Tests.Edit
{
    public static class A
    {
        public static HitInfo HitInfoWith(float hpDamage, float blDamage, float staggerTime = 0)
        {
            return new HitInfo
            {
                Source = HitSource.None,
                DamageStats = new DamageStats
                {
                    BaseBalanceDamage = blDamage,
                    BaseDamage = hpDamage,
                    BasePushForce = 0,
                    BasePushTime = 0,
                    BaseStaggerTime = staggerTime
                },
                Multipliers = DamageMultipliers.One
            };
        }
    }
}