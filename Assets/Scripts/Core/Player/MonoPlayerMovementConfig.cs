using Core.Characters.Mono;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(menuName = "Movement/Player Movement Config", fileName = "PlayerMovementConfig", order = 0)]
    public class MonoPlayerMovementConfig : ScriptableObject
    {
        [UsedImplicitly]
        [field: SerializeField] public PlayerMovementConfig Config { get; private set; }
    }
}