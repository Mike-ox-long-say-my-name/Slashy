using Core;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerManager : PublicSingleton<PlayerManager>
    {
        private BasePlayerData _playerData;

        public BasePlayerData PlayerData
        {
            get
            {
                if (_playerData != null)
                {
                    return _playerData;
                }

                _playerData = FindObjectOfType<BasePlayerData>();
                if (_playerData == null)
                {
                    Debug.LogError("No player found", this);
                }

                return _playerData;
            }
        }
    }
}