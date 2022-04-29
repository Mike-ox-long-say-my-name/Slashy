using UnityEngine;

namespace Core.Characters.Mono
{
    public interface IDamageStatsContainer
    {
        DamageStats DamageStats { get; }
    }
}