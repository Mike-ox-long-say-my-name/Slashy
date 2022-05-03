using Core.Player.Interfaces;
using UnityEngine;

namespace Core.Player
{
    public class PlayerManager : PublicSingleton<PlayerManager>
    {
        private IPlayer _playerData;

        public IPlayer PlayerInfo
        {
            get
            {
                if ((Object)_playerData != null)
                {
                    return _playerData;
                }

                var playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject == null)
                {
                    return null;
                }

                _playerData = playerObject.GetComponent<IPlayer>();
                if (_playerData == null)
                {
                    Debug.LogError($"Player does not implement {nameof(IPlayer)}.\n" +
                                   "Maybe there is multiple players on scene", this);
                }

                return _playerData;
            }
        }
    }
}