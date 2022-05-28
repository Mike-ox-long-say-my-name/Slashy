using System;
using Core.Player.Interfaces;
using UnityEngine;

namespace Core
{
    public interface IPlayerFactory
    {
        event Action<IPlayer> PlayerCreated;
        IPlayer CreatePlayer(PlayerCreationInfo creationInfo);
        void WhenPlayerAvailable(Action<IPlayer> action);
        IPlayer CreatePlayerAtPlayerMarker();
        event Action PlayerDestroyed;
    }
}