using JetBrains.Annotations;
using UnityEngine;

namespace Core.Levels
{
    [CreateAssetMenu(menuName = "Other/Save Data", fileName = "SaveData", order = 1)]
    public class SaveData : ScriptableObject
    {
        [field: SerializeField] public ExitData ExitData { get; [UsedImplicitly] private set; }
        [field: SerializeField] public RespawnData RespawnData { get; [UsedImplicitly] private set; }
    }
}