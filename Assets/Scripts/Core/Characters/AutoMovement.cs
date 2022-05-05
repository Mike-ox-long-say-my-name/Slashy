using System;
using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Characters
{
    public enum AutoMovementMode
    {
        RandomSpherical
    }

    public class AutoMovement : CharacterMovement, IAutoMovement
    {
        private const float DefaultTargetReachedEpsilon = 0.3f;

        private float _targetReachedEpsilon = DefaultTargetReachedEpsilon;
        private Transform _lockOn;
        private Vector3? _targetPosition;
        private Transform _target;
        private float _speedMultiplier = 1;

        public AutoMovement(IMovement movement, MovementConfig movementConfig) : base(movement, movementConfig)
        {
            AutoResetVelocity = true;
        }

        public override void Tick(float deltaTime)
        {
            var desiredPosition = GetTargetPosition();

            if (desiredPosition.HasValue)
            {
                var direction = (desiredPosition.Value - BaseMovement.Transform.position).normalized;
                Move(direction * _speedMultiplier);
            }

            base.Tick(deltaTime);

            if (_lockOn != null)
            {
                var direction = _lockOn.position - BaseMovement.Transform.position;
                BaseMovement.Rotate(direction.x);
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

            var position = BaseMovement.Transform.position;
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