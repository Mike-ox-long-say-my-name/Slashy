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

        private Vector3 _velocity;

        public Vector3 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        private readonly IBaseMovement _baseMovement;

        public VelocityMovement(IBaseMovement baseMovement)
        {
            Guard.NotNull(baseMovement);
            _baseMovement = baseMovement;
        }

        private float _horizontalAirboneVelocity, _verticalAirboneVelocity;

        private void ApplyAirboneVelocity(Vector3 direction)
        {
            _velocity.x = Mathf.SmoothDamp(Velocity.x, HorizontalSpeed * direction.x,
                ref _horizontalAirboneVelocity, AirboneControlFactor);
            _velocity.z = Mathf.SmoothDamp(Velocity.z, VerticalSpeed * direction.z,
                ref _verticalAirboneVelocity, AirboneControlFactor);
        }

        public virtual void Move(Vector3 direction)
        {
            if (_baseMovement.IsGrounded)
            {
                ApplyGroundedVelocity(direction);
            }
            else
            {
                ApplyAirboneVelocity(direction);
            }
        }

        private void ApplyGroundedVelocity(Vector3 direction)
        {
            _velocity.x = direction.x * HorizontalSpeed;
            _velocity.z = direction.z * VerticalSpeed;
        }

        private void ApplyGravity(float deltaTime)
        {
            if (_baseMovement.IsGrounded)
            {
                if (_velocity.y < 0)
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

            _baseMovement.Move(_velocity * deltaTime);
            _baseMovement.Rotate(_velocity.x);

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