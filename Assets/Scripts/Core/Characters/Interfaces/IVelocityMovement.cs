using UnityEngine;

namespace Core.Characters.Interfaces
{
    public interface IVelocityMovement
    {
        IMovement Movement { get; }
        IPushable Pushable { get; }

        Vector3 Velocity { get; }

        void ResetGravity();
        void Move(Vector3 direction);
        void Stop();
        void Tick(float deltaTime);
    }
}