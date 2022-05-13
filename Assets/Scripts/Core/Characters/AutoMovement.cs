using System;
using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Characters
{
    public class AutoMovement : IAutoMovement
    {
        private readonly IVelocityMovement _velocityMovement;

        private const float DefaultTargetReachedEpsilon = 0.3f;

        private float _targetReachedEpsilon = DefaultTargetReachedEpsilon;
        private Transform _lockOn;
        private Transform _target;
        private Vector3? _targetPosition;
        private Vector3 _offset;
        private float _speedMultiplier = 1;

        public bool IgnoreY { get; set; } = true;

        public AutoMovement(IVelocityMovement velocityMovement)
        {
            _velocityMovement = velocityMovement;
        }

        public void Tick(float deltaTime)
        {
            var desiredPosition = GetTargetPosition();

            if (desiredPosition.HasValue)
            {
                var direction = (desiredPosition.Value - _velocityMovement.BaseMovement.Transform.position).normalized;
                _velocityMovement.Move(direction * _speedMultiplier);
            }

            if (_lockOn != null)
            {
                var direction = _lockOn.position - _velocityMovement.BaseMovement.Transform.position;
                _velocityMovement.BaseMovement.Rotate(direction.x);
            }

            CheckTargetReached();
        }

        private void CheckTargetReached()
        {
            var desiredPosition = GetTargetPosition();
            if (!desiredPosition.HasValue)
            {
                return;
            }

            var position = _velocityMovement.BaseMovement.Transform.position;


            var distance = IgnoreY ?
                Vector3.Distance(position.WithZeroY(), desiredPosition.Value.WithZeroY()) :
                Vector3.Distance(position, desiredPosition.Value);

            if (distance >= _targetReachedEpsilon)
            {
                return;
            }

            TargetReached?.Invoke();
            TargetReached = null;
        }

        private Vector3? GetTargetPosition()
        {
            return _target != null ? _target.position + _offset : _targetPosition;
        }

        public void LockRotationOn(Transform lockOn)
        {
            Guard.NotNull(lockOn);
            _lockOn = lockOn;
            _velocityMovement.AutoRotateToDirection = false;
        }

        public void UnlockRotation()
        {
            _lockOn = null;
            _velocityMovement.AutoRotateToDirection = true;
        }

        public void MoveTo(Vector3 position)
        {
            _target = null;
            _targetPosition = position;
        }

        public void MoveToWithOffset(Transform target, Vector3 offset)
        {
            MoveTo(target);
            _offset = offset;
        }

        public void MoveTo(Transform target)
        {
            Guard.NotNull(target);
            _target = target;
            _targetPosition = null;
            _offset = Vector3.zero;
        }

        public void ResetTarget()
        {
            _targetPosition = null;
            _target = null;
            _offset = Vector3.zero;
        }

        public void SetSpeedMultiplier(float speedMultiplier)
        {
            _speedMultiplier = speedMultiplier;
        }

        public void ResetSpeedMultiplier()
        {
            _speedMultiplier = 1;
        }

        public void SetTargetReachedEpsilon(float epsilon)
        {
            _targetReachedEpsilon = epsilon;
        }

        public void ResetTargetReachedEpsilon()
        {
            _targetReachedEpsilon = DefaultTargetReachedEpsilon;
        }

        public event Action TargetReached;
    }
}