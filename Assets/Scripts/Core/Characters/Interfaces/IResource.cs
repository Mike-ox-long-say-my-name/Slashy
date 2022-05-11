using System;

namespace Core.Characters.Interfaces
{
    public interface IResource
    {
        bool Frozen { get; set; }
        float MaxValue { get; }
        float Value { get; set; }

        void Recover(float amount);
        void Spend(float amount);
        void ForceRaiseEvent();

        event Action<IResource> ValueChanged;
    }
}