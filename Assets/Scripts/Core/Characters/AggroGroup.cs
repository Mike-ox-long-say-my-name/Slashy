using System.Collections.Generic;
using UnityEngine;

namespace Core.Characters
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] private float spreadPerSecond = 0.7f;

        [SerializeField] private EnemyMarker[] sharedAggro;

        private readonly List<EnemyConductor> _notAggroed = new List<EnemyConductor>();
        private readonly List<AggroSpreadSpot> _aggroSpreadSpots = new List<AggroSpreadSpot>();

        private void Awake()
        {
            foreach (var enemyMarker in sharedAggro)
            {
                MapEnemyCreation(enemyMarker);
            }
        }

        private void MapEnemyCreation(EnemyMarker enemyMarker)
        {
            if (enemyMarker.CreatedEnemy != null)
            {
                OnCreated(enemyMarker.CreatedEnemy);
            }
            else
            {
                enemyMarker.Created += OnCreated;
            }
        }

        private void OnCreated(EnemyConductor obj)
        {
            _notAggroed.Add(obj);
            obj.Aggro.Aggroed += () => OnAggroed(obj);
        }

        private void OnAggroed(EnemyConductor enemyConductor)
        {
            var spreadSpot = new AggroSpreadSpot
            {
                Location = GetPosition(enemyConductor),
                SpreadRadius = 0
            };

            _aggroSpreadSpots.Add(spreadSpot);
            _notAggroed.Remove(enemyConductor);
        }

        private static Vector3 GetPosition(EnemyConductor enemyConductor)
        {
            return enemyConductor.MovementBase.BaseMovement.Transform.position.WithZeroY();
        }

        private void Update()
        {
            if (_notAggroed.Count == 0)
            {
                return;
            }
            
            TickAggroSpread(Time.deltaTime);
            UpdateAggro();
        }

        private void UpdateAggro()
        {
            var notAggroedCopy = _notAggroed.ToArray();
            foreach (var spot in _aggroSpreadSpots.ToArray())
            {
                foreach (var enemyConductor in notAggroedCopy)
                {
                    if (ShouldAggro(enemyConductor, spot))
                    {
                        enemyConductor.Aggro.Aggro();
                    }
                }
            }
        }

        private static bool ShouldAggro(EnemyConductor enemyConductor, AggroSpreadSpot spot)
        {
            return Vector3.Distance(GetPosition(enemyConductor), spot.Location) < spot.SpreadRadius;
        }

        private void TickAggroSpread(float deltaTime)
        {
            foreach (var spot in _aggroSpreadSpots)
            {
                spot.SpreadRadius += spreadPerSecond * deltaTime;
            }
        }

        private class AggroSpreadSpot
        {
            public Vector3 Location { get; set; }
            public float SpreadRadius { get; set; }
        }

        private void OnDrawGizmos()
        {
            if (sharedAggro == null)
            {
                return;
            }
            
            GizmosHelper.PushColor(Color.blue);

            var sharedAggroLength = sharedAggro.Length;
            for (var i = 0; i < sharedAggroLength; i++)
            {
                var enemyMarker1 = sharedAggro[i];
                var enemyMarker2 = sharedAggro[(i + 1) % sharedAggroLength];
                Gizmos.DrawLine(enemyMarker1.Position, enemyMarker2.Position);
            }
            
            GizmosHelper.PopColor();
        }
    }
}