using JetBrains.Annotations;
using UnityEngine;

namespace Core.Characters.Mono
{
    [CreateAssetMenu(menuName = "Stats/Character Stats", fileName = "CharacterStats", order = 0)]
    public class MonoCharacterStats : ScriptableObject
    {
        [UsedImplicitly]
        [field: SerializeField] public CharacterStats CharacterStats { get; private set; }
    }
}