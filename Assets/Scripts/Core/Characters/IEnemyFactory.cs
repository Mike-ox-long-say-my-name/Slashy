using System;

namespace Core
{
    public interface IEnemyFactory
    {
        void CreateAllAliveAtEnemyMarkersOnLevel();
        void DestroyAllCreated();
        event Action EnemiesDestroyed;
    }
}