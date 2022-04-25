using UnityEngine;

namespace Core.Characters.Interfaces
{
    public interface IRawMovement
    {
        Transform Transform { get; }

        void MoveRaw(Vector3 move);
        void SetPosition(Vector3 position);
    }
}