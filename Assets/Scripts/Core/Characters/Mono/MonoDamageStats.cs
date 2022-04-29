using JetBrains.Annotations;
using UnityEngine;

namespace Core.Characters.Mono
{
    [CreateAssetMenu(menuName = "Stats/Damage Stats", fileName = "DamageStats", order = 0)]
    public class MonoDamageStats : ScriptableObject
    {
        [UsedImplicitly]
        [field: SerializeField] public DamageStats DamageStats { get; private set; }
    }
}