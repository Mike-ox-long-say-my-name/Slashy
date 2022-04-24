using Core.Characters;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerMovement : CharacterMovement, IPlayerMovement
    {
        public IPushable Pushable { get; }

        private readonly IPlayerMovementConfig _config;
        private float _horizontalAirboneVelocity, _verticalAirboneVelocity;
        
        public PlayerMovement(CharacterController controller, IPlayerMovementConfig config) : base(controller, config)
        {
            _config = config;
            Pushable = new Pushable(this);
        }

        private void MoveAirbone(Vector3 direction)
        {
            Velocity.x = Mathf.SmoothDamp(Velocity.x, _config.HorizontalSpeed * direction.x,
                ref _horizontalAirboneVelocity, _config.AirboneControlFactor);
            Velocity.z = Mathf.SmoothDamp(Velocity.z, _config.VerticalSpeed * direction.z,
                ref _verticalAirboneVelocity, _config.AirboneControlFactor);

            Rotate(direction.x);
        }

        public override void Move(Vector3 direction)
        {
            if (IsGrounded)
            {
                base.Move(direction);
            }
            else
            {
                MoveAirbone(direction);
            }
        }

        public void Jump()
        {
            Velocity.y = _config.JumpStartVelocity;
        }
    }
}