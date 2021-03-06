using System;
using System.Collections.Generic;
using System.Linq;
using Core.Levels;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public class EnemyFactory : IEnemyFactory
    {
        private static readonly List<Vector3> DeadEnemies = new List<Vector3>();

        private readonly IObjectLocator _objectLocator;
        private EnemyMarker[] _enemyMarkers;
        private readonly List<EnemyMarker> _createdEnemyMarkers = new List<EnemyMarker>();

        public EnemyFactory(IObjectLocator objectLocator, IGameLoader gameLoader, IRespawnController respawnController)
        {
            _objectLocator = objectLocator;
            gameLoader.Exited += OnExited;
            respawnController.Respawning += OnRespawning;
        }

        private void OnExited()
        {
            _createdEnemyMarkers.Clear();
            DeadEnemies.Clear();
        }

        private static void OnRespawning()
        {
            DeadEnemies.Clear();
        }

        public void CreateAllAliveAtEnemyMarkersOnLevel()
        {
            if (_enemyMarkers == null)
            {
                FindAllEnemyMarkers();
            }

            foreach (var enemyMarker in GetAliveEnemyMarkers())
            {
                var enemy = Object.Instantiate(enemyMarker.Enemy);
                var movement = enemy.MovementBase.BaseMovement;

                movement.SetPosition(enemyMarker.Position);
                movement.Rotate(enemyMarker.Rotation);
                enemyMarker.OnCreated(enemy);

                enemy.Character.Character.Died += _ => OnEnemyDied(enemyMarker);

                _createdEnemyMarkers.Add(enemyMarker);
            }
        }

        private IEnumerable<EnemyMarker> GetAliveEnemyMarkers()
        {
            return _enemyMarkers.Where(marker => !DeadEnemies.Contains(marker.Position));
        }

        private static void OnEnemyDied(EnemyMarker marker)
        {
            DeadEnemies.Add(marker.Position);
        }

        private void FindAllEnemyMarkers()
        {
            _enemyMarkers = _objectLocator.FindAll<EnemyMarker>();
        }

        public void DestroyAllCreated()
        {
            foreach (var enemyMarker in _createdEnemyMarkers)
            {
                var createdEnemy = enemyMarker.CreatedEnemy;
                if (createdEnemy)
                {
                    enemyMarker.CreatedEnemy.Aggro.Deaggro();
                    Object.Destroy(enemyMarker.CreatedEnemy.gameObject);
                }
                DeadEnemies.Remove(enemyMarker.Position);
            }

            _createdEnemyMarkers.Clear();
            EnemiesDestroyed?.Invoke();
        }

        public void RecreateAllEnemiesOnLevel()
        {
            DestroyAllCreated();
            CreateAllAliveAtEnemyMarkersOnLevel();
        }

        public event Action EnemiesDestroyed;
    }
}