using System;
using System.Linq;
using Characters.Player.States;
using Core.Player.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public class PlayerFactory : IPlayerFactory
    {
        public event Action<IPlayer> PlayerCreated;
        public event Action PlayerDestroyed;
        
        private const string PlayerPath = "Player";

        private readonly IObjectLocator _objectLocator;


        private static PlayerStateMachine _playerPrefab;
        private IPlayer _player;

        public PlayerFactory(IObjectLocator objectLocator, ISceneLoader sceneLoader)
        {
            _objectLocator = objectLocator;

            sceneLoader.SceneUnloaded += OnSceneUnloaded;
            LoadResources();
        }

        private void OnSceneUnloaded(string _)
        {
            _player = null;
            PlayerDestroyed?.Invoke();
        }

        private static void LoadResources()
        {
            if (!_playerPrefab)
            {
                _playerPrefab = Resources.Load<PlayerStateMachine>(PlayerPath);
            }
        }

        public IPlayer CreatePlayerAtPlayerMarker()
        {
            var playerMarker = _objectLocator.FindAll<PlayerMarker>().First();
            
            _player = Object.Instantiate(_playerPrefab);
            Configure(playerMarker.Position, playerMarker.Rotation);
            PlayerCreated?.Invoke(_player);
            return _player;
        }

        private void Configure(Vector3 position, float rotation)
        {
            var movement = _player.VelocityMovement.BaseMovement;
            movement.SetPosition(position);
            movement.Rotate(rotation);
        }

        public IPlayer CreatePlayer(PlayerCreationInfo creationInfo)
        {
            _player = Object.Instantiate(_playerPrefab);
            Configure(creationInfo);
            PlayerCreated?.Invoke(_player);
            return _player;
        }

        public void WhenPlayerAvailable(Action<IPlayer> action)
        {
            if (_player != null)
            {
                action(_player);
            }
            else
            {
                PlayerCreated += action;
            }
        }

        private void Configure(PlayerCreationInfo creationInfo)
        {
            _player.Character.Health.Value = creationInfo.Health;
            _player.Character.Balance.Value = creationInfo.Balance;
            _player.Stamina.Value = creationInfo.Stamina;
            var movement = _player.VelocityMovement.BaseMovement;
            movement.SetPosition(creationInfo.Position);
        }
    }
}