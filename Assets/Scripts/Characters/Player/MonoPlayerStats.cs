using JetBrains.Annotations;
using UnityEngine;

namespace Characters.Player
{
    [CreateAssetMenu(menuName = "Stats/Player Stats", fileName = "PlayerAdditionalStats", order = 0)]
    public class MonoPlayerStats : ScriptableObject
    {
        [UsedImplicitly]
        [field: SerializeField] public PlayerStats PlayerStats { get; private set; }
    }
}