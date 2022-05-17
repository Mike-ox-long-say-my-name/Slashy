using Core.Player;
using UnityEngine;

namespace Core.Levels
{
    public class RespawnManager : PublicSingleton<RespawnManager>
    {
        [SerializeField] private Vector3 defaultRespawnPosition = Vector3.zero;
        [SerializeField] private RespawnData respawnData;

        private bool _isRespawning = false;

        private void Start()
        {
            GameLoader.Instance.StartingNewGame.AddListener(OnStartingNewGame);
            GameLoader.Instance.LoadedExitedLevel.AddListener(OnLoadedExitedLevel);
            PlayerManager.Instance.PlayedDeadSequenceEnded.AddListener(Respawn);
            PlayerManager.Instance.PlayerLoaded.AddListener(OnPlayerLoaded);
        }

        private void OnLoadedExitedLevel(string arg0)
        {
            _isRespawning = true;
        }

        private void OnStartingNewGame(string level)
        {
            respawnData.RespawnLevel = level;
            respawnData.RespawnPosition = defaultRespawnPosition;
        }

        private void OnPlayerLoaded()
        {
            if (!_isRespawning)
            {
                return;
            }

            _isRespawning = false;
            var player = PlayerManager.Instance.PlayerInfo;
            player.VelocityMovement.BaseMovement.SetPosition(respawnData.RespawnPosition);
        }

        public void Respawn()
        {
            _isRespawning = true;
            var level = string.IsNullOrEmpty(respawnData.RespawnLevel)
                ? GameLoader.GetCurrentScene()
                : respawnData.RespawnLevel;
            GameLoader.Instance.LoadLevel(level);
        }
        
        public void UpdateRespawnData(Bonfire bonfire)
        {
            respawnData.RespawnLevel = GameLoader.GetCurrentScene();
            respawnData.RespawnPosition = bonfire.GetRespawnPosition();
        }
    }
}