using System;

namespace Core.Levels
{
    public interface IRespawnController
    {
        void UpdateRespawnData(Bonfire bonfire);
        event Action Respawning;
    }
}