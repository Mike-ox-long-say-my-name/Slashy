using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Characters
{
    public class VelocityMovement : IVelocityMovement
    {
        public float HorizontalSpeed { get; set; }
        public float VerticalSpeed { get; set; }
        public float MinVelocity { get; set; }
        public float MaxVelocity { get; set; }
        public float Gravity { get; set; }
        public float GroundedGravity { get; set; }
        public float AirboneControlFactor { get; set; }

        public bool AutoResetVelocity { get; set; } = true;
        public bool AutoRotateToDirection { get; set; } = true;

        public float MinMoveDistance { get; set; } = 0.001f;

        private Vector3 _velocity;

        public Vector3 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        public IBaseMovement BaseMovement { get; }

        public VelocityMovement(IBaseMovement baseMovement)
        {
            Guard.NotNull(baseMovement);
            BaseMovement = baseMovement;
        }

        private float _horizontalAirboneVelocity;
        private float _verticalAirboneVelocity;

        private void ApplyAirboneVelocity(Vector3 direction)
        {
            _velocity.x = Mathf.SmoothDamp(Velocity.x, HorizontalSpeed * direction.x,
                ref _horizontalAirboneVelocity, AirboneControlFactor);
            _velocity.z = Mathf.SmoothDamp(Velocity.z, VerticalSpeed * direction.z,
                ref _verticalAirboneVelocity, AirboneControlFactor);
        }

        public virtual void Move(Vector3 direction)
        {
            if (BaseMovement.IsGrounded)
            {
                ApplyGroundedVelocity(direction);
                ResetAirboneValues();
            }
            else
            {
                ApplyAirboneVelocity(direction);
            }
        }

        private void ResetAirboneValues()
        {
            _horizontalAirboneVelocity = 0;
            _verticalAirboneVelocity = 0;
        }

        private void ApplyGroundedVelocity(Vector3 direction)
        {
            _velocity.x = direction.x * HorizontalSpeed;
            _velocity.z = direction.z * VerticalSpeed;
        }

        private void ApplyGravity(float deltaTime)
        {
            if (BaseMovement.IsGrounded)
            {
                if (_velocity.y <= 1e-5)
                {
                    _velocity.y = GroundedGravity;
                }
            }
            else
            {
                _velocity.y += Gravity * deltaTime;
            }
        }


        public virtual void Tick(float deltaTime)
        {
            ApplyGravity(deltaTime);
            ClampVelocity();

            var move = _velocity * deltaTime;
            if (move.magnitude > MinMoveDistance)
            {
                BaseMovement.Move(_velocity * deltaTime);
            }
            if (AutoRotateToDirection)
            {
                BaseMovement.Rotate(_velocity.x);
            }

            if (!AutoResetVelocity)
            {
                return;
            }

            _velocity.x = 0;
            _velocity.z = 0;
        }

        private void ClampVelocity()
        {
            var minVelocity = MinVelocity;
            var maxVelocity = MaxVelocity;

            _velocity.x = Mathf.Clamp(_velocity.x, minVelocity, maxVelocity);
            _velocity.y = Mathf.Clamp(_velocity.y, minVelocity, maxVelocity);
            _velocity.z = Mathf.Clamp(_velocity.z, minVelocity, maxVelocity);
        }
    }
}