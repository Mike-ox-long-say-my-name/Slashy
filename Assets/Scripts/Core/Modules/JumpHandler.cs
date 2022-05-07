using Core.Characters.Interfaces;

namespace Core.Modules
{
    public class JumpHandler : IJumpHandler
    {
        private readonly IBaseMovement _baseBaseMovement;
        private readonly IVelocityMovement _velocityMovement;
        private readonly float _jumpPower;

        public JumpHandler(IBaseMovement baseBaseMovement, IVelocityMovement velocityMovement, float jumpPower)
        {
            _baseBaseMovement = baseBaseMovement;
            _velocityMovement = velocityMovement;
            _jumpPower = jumpPower;
        }

        public void Jump()
        {
            if (CanJump())
            {
                ApplyVelocity();
            }
        }

        private bool CanJump()
        {
            return _baseBaseMovement.IsGrounded;
        }

        private void ApplyVelocity()
        {
            var velocity = _velocityMovement.Velocity;
            velocity.y = _jumpPower;
            _velocityMovement.Velocity = velocity;
        }
    }
}