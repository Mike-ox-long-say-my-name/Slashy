using System;

namespace Core.Characters.Interfaces
{
    public interface ICharacterResource
    {
        ICharacter Character { get; }

        float MaxValue { get; }
        float Value { get; set; }

        void Recover(float amount);
        void Spend(float amount);

        event Action<ICharacterResource> OnValueChanged;
    }
}