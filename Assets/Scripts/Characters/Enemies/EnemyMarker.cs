using System;
using Core.Modules;
using UnityEngine;

namespace Core
{
    public class EnemyMarker : Marker
    {
        [SerializeField] private MixinMovementBase enemy;

        public MixinMovementBase Enemy => enemy;

        public event Action<MixinMovementBase> Created;

        public void OnCreated(MixinMovementBase obj)
        {
            Created?.Invoke(obj);
        }
    }
}