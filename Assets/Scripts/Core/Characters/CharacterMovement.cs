using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters
{
    public class CharacterMovement : ICharacterMovement
    {
        public Transform Transform { get; }
        public CharacterController Controller { get; }

        protected Vector3 Velocity;
        Vector3 ICharacterMovement.Velocity => Velocity;

        private readonly ICharacterMovementConfig _config;
        private readonly IPushable _pushable;

        public CharacterMovement(CharacterController controller, ICharacterMovementConfig config)
        {
            Controller = controller;
            Transform = controller.transform;
            _config = config;
            _pushable = new Pushable(this);
        }

        public void ResetGravity()
        {
            Velocity.y = 0;
        }

        public void MoveRaw(Vector3 move)
        {
            Controller.Move(move);
        }

        public void SetPosition(Vector3 position)
        {
            Controller.enabled = false;
            Transform.position = position;
            Controller.enabled = true;
        }

        public bool IsGrounded => Controller.isGrounded;
        public bool IsFalling => Velocity.y < 0;

        public virtual void Move(Vector3 direction)
        {
            Velocity.x = direction.x * _config.HorizontalSpeed;
            Velocity.z = direction.z * _config.VerticalSpeed;

            Rotate(direction.x);
        }

        public virtual void Stop()
        {
            Velocity.x = 0;
            Velocity.z = 0;
        }

        public virtual void Rotate(float direction)
        {
            if (Mathf.Abs(direction) > 0)
            {
                Transform.eulerAngles = new Vector3(0, direction > 0 ? 0 : 180, 0);
            }
        }

        public virtual void Tick(float deltaTime)
        {
            if (_pushable.IsPushing)
            {
                _pushable.Tick(deltaTime);
                return;
            }

            Velocity.y += (IsGrounded ? _config.GroundedGravity : _config.Gravity) * deltaTime;
            ClampVelocity();
            MoveRaw(Velocity * deltaTime);
        }

        private void ClampVelocity()
        {
            var minVelocity = _config.MinVelocity;
            var maxVelocity = _config.MaxVelocity;

            Velocity.x = Mathf.Clamp(Velocity.x, minVelocity, maxVelocity);
            Velocity.y = Mathf.Clamp(Velocity.y, minVelocity, maxVelocity);
            Velocity.z = Mathf.Clamp(Velocity.z, minVelocity, maxVelocity);
        }

    }
}