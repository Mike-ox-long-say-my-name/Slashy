using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public interface IEnemyFactory
    {
        void CreateAllOnLevelAtEnemyMarkers();
        void DestroyAllCreated();
        event Action EnemiesDestroyed;
    }

    public class EnemyFactory : IEnemyFactory
    {
        private readonly IObjectLocator _objectLocator;
        private EnemyMarker[] _enemyMarkers;
        private readonly List<GameObject> _createdEnemies = new List<GameObject>();

        public EnemyFactory(IObjectLocator objectLocator)
        {
            _objectLocator = objectLocator;
        }

        public void CreateAllOnLevelAtEnemyMarkers()
        {
            if (_enemyMarkers == null)
            {
                FindAllEnemyMarkers();
            }

            foreach (var enemyMarker in _enemyMarkers)
            {
                var enemy = Object.Instantiate(enemyMarker.Enemy);
                enemy.BaseMovement.SetPosition(enemyMarker.Position);
                enemy.BaseMovement.Rotate(enemyMarker.Rotation);
                enemyMarker.OnCreated(enemy);
                
                _createdEnemies.Add(enemy.gameObject);
            }
        }

        private void FindAllEnemyMarkers()
        {
            _enemyMarkers = _objectLocator.FindAll<EnemyMarker>();
        }

        public void DestroyAllCreated()
        {
            foreach (var enemy in _createdEnemies)
            {
                Object.Destroy(enemy);
            }
            _createdEnemies.Clear();
            EnemiesDestroyed?.Invoke();
        }

        public event Action EnemiesDestroyed;
    }
}