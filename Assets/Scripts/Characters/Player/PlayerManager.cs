using Core;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerManager : PersistentSingleton<PlayerManager>
    {
        public BasePlayerData BasePlayerData { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            BasePlayerData = FindObjectOfType<BasePlayerData>();

            if (BasePlayerData == null)
            {
                Debug.LogError("No player found", this);
            }
        }
    }
}