using UnityEngine;

namespace Core.Levels
{
    public class SaveDataContainer : MonoBehaviour
    {
        [SerializeField] private RespawnData respawnData;
        [SerializeField] private BonfireSaveData bonfireSaveData;

        public RespawnData RespawnData => respawnData;

        public BonfireSaveData BonfireSaveData => bonfireSaveData;
    }
}