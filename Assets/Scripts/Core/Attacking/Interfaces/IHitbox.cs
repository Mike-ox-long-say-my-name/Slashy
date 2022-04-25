using UnityEngine;

namespace Core.Attacking.Interfaces
{
    public interface IHitbox
    {
        Transform Transform { get; }

        bool IsEnabled { get; }

        void Enable();
        void Disable();
    }
}