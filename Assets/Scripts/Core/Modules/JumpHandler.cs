using Core.Characters.Interfaces;

namespace Core.Modules
{
    public class JumpHandler : IJumpHandler
    {
        private readonly IBaseMovement _baseMovement;
        private readonly IVelocityMovement _velocityMovement;
        private readonly float _jumpPower;

        public JumpHandler(IBaseMovement baseMovement, IVelocityMovement velocityMovement, float jumpPower)
        {
            _baseMovement = baseMovement;
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
            return _baseMovement.IsGrounded;
        }

        private void ApplyVelocity()
        {
            var velocity = _velocityMovement.Velocity;
            velocity.y = _jumpPower;
            _velocityMovement.Velocity = velocity;
        }
    }
}