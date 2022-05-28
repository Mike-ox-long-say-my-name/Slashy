using System;
using Core.Characters;
using UnityEngine;

namespace Core
{
    public class EnemyMarker : Marker
    {
        [SerializeField] private EnemyConductor enemy;

        public EnemyConductor Enemy => enemy;

        public EnemyConductor CreatedEnemy { get; private set; }

        public event Action<EnemyConductor> Created;

        public void OnCreated(EnemyConductor obj)
        {
            CreatedEnemy = obj;
            CreatedEnemy.gameObject.SetActive(true);
            Created?.Invoke(CreatedEnemy);
        }
    }
}