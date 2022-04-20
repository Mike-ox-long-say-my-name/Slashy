using Core.Characters;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerMovement : CharacterMovement
    {
        [SerializeField, Min(0)] private float jumpStartVelocity = 5;
        [SerializeField, Range(0, 1)] private float airboneControlFactor;

        private float _horizontalAirboneVelocity, _verticalAirboneVelocity;

        private void MoveAirbone(Vector2 input)
        {
            var velocity = Velocity;
            velocity.x = Mathf.SmoothDamp(velocity.x, HorizontalSpeed * input.x,
                ref _horizontalAirboneVelocity, airboneControlFactor);
            velocity.z = Mathf.SmoothDamp(velocity.z, VerticalSpeed * input.y,
                ref _verticalAirboneVelocity, airboneControlFactor);
            Velocity = velocity;

            HandleRotation(input.x);
        }

        public override void Move(Vector2 input)
        {
            if (IsGrounded)
            {
                base.Move(input);
            }
            else
            {
                MoveAirbone(input);
            }
        }

        public void Jump()
        {
            var velocity = Velocity;
            velocity.y = jumpStartVelocity;
            Velocity = velocity;
        }
    }
}