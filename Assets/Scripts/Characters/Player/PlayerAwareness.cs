using System;

namespace Characters.Player
{
    public class PlayerAwareness : IPlayerAwareness
    {
        public event Action Danger;

        public void PostDangerEvent()
        {
            Danger?.Invoke();
        }
    }

}