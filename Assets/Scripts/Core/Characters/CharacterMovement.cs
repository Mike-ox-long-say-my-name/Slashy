using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Characters
{
    public class CharacterMovement : IVelocityMovement
    {
        public MovementConfig MovementConfig { get; }

        protected Vector3 Velocity;
        public IMovement BaseMovement { get; }
        public IPushable Pushable { get; }

        Vector3 IVelocityMovement.Velocity => Velocity;

        public CharacterMovement(IMovement movement, MovementConfig movementConfig)
        {
            Guard.NotNull(movement);
            Guard.NotNull(movementConfig);

            BaseMovement = movement;
            Pushable = new Pushable(movement);
            MovementConfig = movementConfig;
        }

        public void ResetGravity()
        {
            Velocity.y = 0;
        }

        public virtual void Move(Vector3 direction)
        {
            Velocity.x = direction.x * MovementConfig.HorizontalSpeed;
            Velocity.z = direction.z * MovementConfig.VerticalSpeed;
        }

        public virtual void Stop()
        {
            Velocity.x = 0;
            Velocity.z = 0;
        }

        public virtual void Tick(float deltaTime)
        {
            if (Pushable.IsPushing)
            {
                Pushable.Tick(deltaTime);
                return;
            }

            Velocity.y += (BaseMovement.IsGrounded ? MovementConfig.GroundedGravity : MovementConfig.Gravity) * deltaTime;
            ClampVelocity();
            BaseMovement.Move(Velocity * deltaTime);
            
            BaseMovement.Rotate(Velocity.x);
        }

        private void ClampVelocity()
        {
            var minVelocity = MovementConfig.MinVelocity;
            var maxVelocity = MovementConfig.MaxVelocity;

            Velocity.x = Mathf.Clamp(Velocity.x, minVelocity, maxVelocity);
            Velocity.y = Mathf.Clamp(Velocity.y, minVelocity, maxVelocity);
            Velocity.z = Mathf.Clamp(Velocity.z, minVelocity, maxVelocity);
        }

    }
}