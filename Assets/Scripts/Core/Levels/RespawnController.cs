using System;
using Core.Player.Interfaces;
using UnityEngine;

namespace Core.Levels
{
    public class RespawnController : IRespawnController
    {
        private static readonly Vector3 DefaultRespawnPosition = Vector3.zero;

        private readonly ISceneLoader _sceneLoader;
        private readonly IGameLoader _gameLoader;
        private readonly RespawnData _respawnData;
        
        private bool _isRespawning;

        public RespawnController(ISceneLoader sceneLoader, IGameLoader gameLoader,
            IPlayerDeathSequencePlayer deathSequencePlayer, IPlayerFactory playerFactory,
            SaveDataContainer saveDataContainer)
        {
            _sceneLoader = sceneLoader;
            _gameLoader = gameLoader;
            _respawnData = saveDataContainer.RespawnData;
            
            SubscribeToGameLoaderEvents();
            MapRespawnCallbacks();

            void SubscribeToGameLoaderEvents()
            {
                _gameLoader.StartingNewGame += OnStartingNewGame;
                _gameLoader.LoadedExitedLevel += OnLoadedExitedLevel;
                _gameLoader.GameCompleted += OnGameCompleted;
            }

            void MapRespawnCallbacks()
            {
                deathSequencePlayer.DeathSequenceEnded += OnDeathSequenceEnded;
                playerFactory.WhenPlayerAvailable(OnPlayerAvailable);
            }
        }

        private void OnPlayerAvailable(IPlayer player)
        {
            if (!_isRespawning)
            {
                return;
            }

            EndRespawn(player);
        }

        private void EndRespawn(IPlayer player)
        {
            _isRespawning = false;
            player.VelocityMovement.BaseMovement.SetPosition(_respawnData.RespawnPosition);
        }

        private void OnGameCompleted()
        {
            _respawnData.RespawnLevel = string.Empty;
            _respawnData.RespawnPosition = DefaultRespawnPosition;
        }

        private void OnLoadedExitedLevel(string _)
        {
            _isRespawning = true;
        }

        private void OnStartingNewGame(string level)
        {
            _respawnData.RespawnLevel = level;
            _respawnData.RespawnPosition = DefaultRespawnPosition;
        }

        private void OnDeathSequenceEnded()
        {
            StartRespawn();
        }

        private void StartRespawn()
        {
            if (_isRespawning)
            {
                return;
            }

            _isRespawning = true;
            Respawning?.Invoke();
            var level = string.IsNullOrEmpty(_respawnData.RespawnLevel)
                ? _sceneLoader.CurrentSceneName
                : _respawnData.RespawnLevel;
            _gameLoader.LoadLevel(level);
        }

        public void UpdateRespawnData(Bonfire bonfire)
        {
            _respawnData.RespawnLevel = _sceneLoader.CurrentSceneName;
            _respawnData.RespawnPosition = bonfire.GetRespawnPosition();
        }

        public event Action Respawning;
    }
}