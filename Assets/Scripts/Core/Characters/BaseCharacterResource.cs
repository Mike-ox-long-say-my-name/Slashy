using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters
{
    public abstract class BaseCharacterResource : ICharacterResource
    {
        public ICharacter Character { get; }

        public float MaxValue { get; }
        public float Value { get; protected set; }

        protected BaseCharacterResource(ICharacter character, float maxValue, float startValue)
        {
            Character = character;
            MaxValue = maxValue;
            Value = startValue;
        }

        protected BaseCharacterResource(ICharacter character, float maxValue) : this(character, maxValue, maxValue)
        {
        }

        public virtual void Spend(float amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Cannot spend negative resource amount");
                return;
            }
            Value = Mathf.Max(0, Value - amount);
        }

        public virtual void Recover(float amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Cannot spend negative resource amount");
                return;
            }
            Value = Mathf.Min(MaxValue, Value + amount);
        }

        public bool IsDepleted => Mathf.Approximately(Value, 0);
    }
}