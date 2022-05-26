using System;
using Characters.Player.States;
using Core.Player.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public class PlayerFactory : IPlayerFactory
    {
        private const string PlayerPath = "Player";
        
        public event Action<IPlayer> PlayerCreated;
        
        private static PlayerStateMachine _playerPrefab;
        private IPlayer _player;

        public PlayerFactory()
        {
            LoadResources();
        }

        private static void LoadResources()
        {
            if (!_playerPrefab)
            {
                _playerPrefab = Resources.Load<PlayerStateMachine>(PlayerPath);
            }
        }

        public IPlayer CreatePlayer(PlayerCreationInfo creationInfo)
        {
            _player = Object.Instantiate(_playerPrefab);
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
    }
}