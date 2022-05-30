using UnityEngine;

namespace Core.Levels
{
    public class LevelWarper : ILevelWarper
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IGameLoader _gameLoader;
        private readonly IBlackScreenService _blackScreenService;
        private readonly IPlayerFactory _playerFactory;

        private const float LoadDelay = 2;

        public LevelWarper(ICoroutineRunner coroutineRunner, IGameLoader gameLoader,
            IBlackScreenService blackScreenService, IPlayerFactory playerFactory)
        {
            _coroutineRunner = coroutineRunner;
            _gameLoader = gameLoader;
            _blackScreenService = blackScreenService;
            _playerFactory = playerFactory;
        }

        public bool CanInitiateWarp { get; private set; } = true;

        public void InitiateWarp(Vector3 position, LevelWarpInfo info)
        {
            if (!CanInitiateWarp)
            {
                return;
            }

            CanInitiateWarp = false;

            var player = _playerFactory.GetLazyPlayer().Value;
            player.StartWarp(position);

            _blackScreenService.Blackout(LoadDelay);
            _coroutineRunner.RunAfter(() => LoadNextLevel(info), LoadDelay);
        }

        public void ConfirmWarpEnd()
        {
            var player = _playerFactory.GetLazyPlayer().Value;
            player.EndWarp(_transferData.WarpTargetPosition);
            
            _blackScreenService.Whiteout(LoadDelay);
            _coroutineRunner.RunAfter(() => CanInitiateWarp = true, LoadDelay / 2);
        }

        private void LoadNextLevel(LevelWarpInfo info)
        {
            var player = _playerFactory.GetLazyPlayer().Value;
            _transferData = new PlayerTransferData
            {
                CreationInfo = new PlayerCreationInfo
                {
                    Health = player.Character.Health.Value,
                    Balance = player.Character.Balance.Value,
                    Stamina = player.Stamina.Value,
                    Purity = player.Purity.Value,
                    Position = info.StartPosition,
                },
                WarpTargetPosition = info.StartMoveTarget
            };

            _gameLoader.LoadLevel(info.LevelName);
        }

        private struct PlayerTransferData
        {
            public PlayerCreationInfo CreationInfo;
            public Vector3 WarpTargetPosition;
        }

        private PlayerTransferData _transferData;

        public bool IsWarping => !CanInitiateWarp;
        public PlayerCreationInfo CreationInfo => _transferData.CreationInfo;
    }
}