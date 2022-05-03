using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Player;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerMovement : CharacterMovement, IPlayerMovement
    {
        public PlayerMovementConfig PlayerMovementConfig { get; set; }
        private float _horizontalAirboneVelocity, _verticalAirboneVelocity;
        
        public PlayerMovement(IMovement movement, MovementConfig movementConfig,
            PlayerMovementConfig playerMovementConfig) : base(movement, movementConfig)
        {
            PlayerMovementConfig = playerMovementConfig;
        }

        private void MoveAirbone(Vector3 direction)
        {
            Velocity.x = Mathf.SmoothDamp(Velocity.x, MovementConfig.HorizontalSpeed * direction.x,
                ref _horizontalAirboneVelocity, PlayerMovementConfig.AirboneControlFactor);
            Velocity.z = Mathf.SmoothDamp(Velocity.z, MovementConfig.VerticalSpeed * direction.z,
                ref _verticalAirboneVelocity, PlayerMovementConfig.AirboneControlFactor);
        }

        public override void Move(Vector3 direction)
        {
            if (BaseMovement.IsGrounded)
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
            Velocity.y = PlayerMovementConfig.JumpStartVelocity;
        }
    }
}