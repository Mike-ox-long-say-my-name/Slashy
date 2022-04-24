using UnityEngine;

namespace Core.Characters
{
    public interface ICharacterMovement : IRawMovement
    {
        CharacterController Controller { get; }

        Vector3 Velocity { get; }
        bool IsGrounded { get; }
        bool IsFalling { get; }

        void Move(Vector3 direction);
        void Stop();
        void Rotate(float direction);
        void Tick(float deltaTime);
    }
}