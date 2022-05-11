using Core.Player.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Player
{
    public class PlayerManager : PublicSingleton<PlayerManager>
    {
        private IPlayer _playerInfo;

        public IPlayer PlayerInfo
        {
            get
            {
                if ((Object)_playerInfo != null)
                {
                    return _playerInfo;
                }

                var playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject == null)
                {
                    return null;
                }

                _playerInfo = playerObject.GetComponent<IPlayer>();
                if (_playerInfo == null)
                {
                    Debug.LogError($"Player does not implement {nameof(IPlayer)}.\n" +
                                   "Maybe there is multiple players on scene", this);
                }

                return _playerInfo;
            }
        }

        [SerializeField] private UnityEvent playerLoaded;
        [SerializeField] private UnityEvent playedDeadSequenceStarted;
        [SerializeField] private UnityEvent playedDeadSequenceEnded;

        public UnityEvent PlayerLoaded => playerLoaded;
        public UnityEvent PlayedDeadSequenceStarted => playedDeadSequenceStarted;
        public UnityEvent PlayedDeadSequenceEnded => playedDeadSequenceEnded;

        public void ActivatePlayer()
        {
            TrySetActive(true);
        }

        public void DeactivatePlayer()
        {
            TrySetActive(false);
        }

        private void TrySetActive(bool value)
        {
            var player = PlayerInfo;
            if (player == null)
            {
                return;
            }

            var playerObject = player.PlayerObject;
            if (playerObject == null)
            {
                return;
            }

            playerObject.SetActive(value);
        }
    }
}