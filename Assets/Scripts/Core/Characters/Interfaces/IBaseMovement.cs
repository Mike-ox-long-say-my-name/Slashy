using UnityEngine;

namespace Core.Characters.Interfaces
{
    public interface IBaseMovement
    {
        bool IsGrounded { get; }

        Transform Transform { get; }

        void Move(Vector3 move);
        void Rotate(float direction);
        void SetPosition(Vector3 position);
    }
}