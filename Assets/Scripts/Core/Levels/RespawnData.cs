using UnityEngine;

namespace Core.Levels
{
    [CreateAssetMenu(menuName = "Other/Respawn Data", fileName = "RespawnData", order = 3)]
    public class RespawnData : ScriptableObject
    {
        [field: SerializeField] public Vector3 RespawnPosition { get; set; }
        [field: SerializeField] public string RespawnLevel { get; set; }

        public void ResetData()
        {
            RespawnPosition = Vector3.zero;
            RespawnLevel = string.Empty;
        }
    }
}