using System;

namespace Characters.Player
{
    public interface IPlayerAwareness
    {
        event Action Danger;
        void PostDangerEvent();
    }
}