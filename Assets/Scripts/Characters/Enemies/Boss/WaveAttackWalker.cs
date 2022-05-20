using Core;
using Core.Characters.Interfaces;
using Core.Modules;
using Core.Utilities;
using System;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    [RequireComponent(typeof(MixinMovementBase))]
    [RequireComponent(typeof(MixinDestroyable))]
    public class WaveAttackWalker : MonoBehaviour
    {
        private IBaseMovement _movement;
        private Timer _moveTimer;
        private Action<Vector3> _moved;
        private Vector3 _direction;

        private MixinDestroyable _destroyable;

        private void Awake()
        {
            _movement = GetComponent<MixinMovementBase>().BaseMovement;
            _destroyable = GetComponent<MixinDestroyable>();
            _destroyable.DestroyLater();
        }

        public void GoInDirection(Vector3 direction, float moveInterval, Action<Vector3> moved)
        {
            _moved = moved;
            _direction = direction;
            _moveTimer = Timer.Start(moveInterval, OnTimeout, true);
        }

        private void Update()
        {
            _moveTimer?.Tick(Time.deltaTime);
        }

        private void OnTimeout()
        {
            var flags = _movement.MoveWithFlags(_direction);
            if ((flags & CollisionFlags.CollidedSides) != 0)
            {
                _destroyable.Destroy();
            }
            else
            {
                _moved?.Invoke(transform.position);
            }
        }
    }
}