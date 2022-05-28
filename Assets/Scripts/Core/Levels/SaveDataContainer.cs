using UI.PopupHints;
using UnityEngine;

namespace Core.Levels
{
    public class SaveDataContainer : MonoBehaviour
    {
        [SerializeField] private RespawnData respawnData;
        [SerializeField] private BonfireSaveData bonfireSaveData;
        [SerializeField] private ShownHintsSO shownHints;

        public RespawnData RespawnData => respawnData;
        public BonfireSaveData BonfireSaveData => bonfireSaveData;
        public ShownHintsSO ShownHints => shownHints;
    }
}