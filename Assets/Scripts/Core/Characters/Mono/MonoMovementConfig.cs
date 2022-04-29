using JetBrains.Annotations;
using UnityEngine;

namespace Core.Characters.Mono
{
    [CreateAssetMenu(menuName = "Movement/Movement Config", fileName = "MovementConfig", order = 0)]
    public class MonoMovementConfig : ScriptableObject
    {
        [UsedImplicitly]
        [field: SerializeField] public MovementConfig Config { get; private set; }
    }
}