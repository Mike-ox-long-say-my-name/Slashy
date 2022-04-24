using Characters.Player.States;
using Core;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerManager : PublicSingleton<PlayerManager>
    {
        private IPlayer _playerData;

        public IPlayer Player
        {
            get
            {
                if ((Object)_playerData != null)
                {
                    return _playerData;
                }
                _playerData = FindObjectOfType<PlayerStateMachine>();

                if ((Object)_playerData == null)
                {
                    Debug.LogError("No player found", this);
                }

                return _playerData;
            }
        }
    }
}