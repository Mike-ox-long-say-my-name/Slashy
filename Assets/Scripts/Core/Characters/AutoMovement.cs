using System;
using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Characters
{
    public class AutoMovement : IAutoMovement
    {
        private readonly Transform _transform;
        private readonly IBaseMovement _baseBaseMovement;
        private readonly IVelocityMovement _velocityMovement;

        private const float DefaultTargetReachedEpsilon = 0.3f;

        private float _targetReachedEpsilon = DefaultTargetReachedEpsilon;
        private Transform _lockOn;
        private Transform _target;
        private Vector3? _targetPosition;
        private float _speedMultiplier = 1;

        public AutoMovement(Transform transform, IBaseMovement baseBaseMovement, IVelocityMovement velocityVelocityMovement)
        {
            _transform = transform;
            _baseBaseMovement = baseBaseMovement;
            _velocityMovement = velocityVelocityMovement;
        }

        public void Tick(float deltaTime)
        {
            var desiredPosition = GetTargetPosition();

            if (desiredPosition.HasValue)
            {
                var direction = (desiredPosition.Value - _transform.position).normalized;
                _velocityMovement.Move(direction * _speedMultiplier);
            }

            if (_lockOn != null)
            {
                var direction = _lockOn.position - _transform.position;
                _baseBaseMovement.Rotate(direction.x);
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

            var position = _transform.position;
            var distance = Vector3.Distance(position, desiredPosition.Value);
            if (distance < _targetReachedEpsilon)
            {
                TargetReached?.Invoke();
                TargetReached = null;
            }
        }

        private Vector3? GetTargetPosition()
        {
            return _target != null ? _target.position : _targetPosition;
        }

        public void LockRotationOn(Transform lockOn)
        {
            Guard.NotNull(lockOn);
            _lockOn = lockOn;
        }

        public void UnlockRotation()
        {
            _lockOn = null;
        }

        public void MoveTo(Vector3 position)
        {
            _target = null;
            _targetPosition = position;
        }

        public void MoveTo(Transform target)
        {
            Guard.NotNull(target);
            _target = target;
            _targetPosition = null;
        }

        public void ResetTarget()
        {
            _targetPosition = null;
            _target = null;
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