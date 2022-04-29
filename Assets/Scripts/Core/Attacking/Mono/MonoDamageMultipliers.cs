using JetBrains.Annotations;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [CreateAssetMenu(menuName = "Damage Multipliers", fileName = "DamageMultipliers", order = 0)]
    public class MonoDamageMultipliers : ScriptableObject
    {
        [UsedImplicitly]
        [field: SerializeField] public DamageMultipliers DamageMultipliers { get; private set; }
    }
}