using System;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters
{
    public abstract class BaseCharacterResource : ICharacterResource
    {
        public event Action<ICharacterResource> OnValueChanged;

        public ICharacter Character { get; }

        private float _value;

        public float MaxValue { get; }

        public float Value
        {
            get => _value;
            set
            {
                var newValue = Mathf.Clamp(value, 0, MaxValue);
                if (Mathf.Approximately(newValue, _value))
                {
                    return;
                }

                _value = newValue;
                OnValueChanged?.Invoke(this);
            }
        }

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

            Value -= amount;
        }

        public virtual void Recover(float amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Cannot spend negative resource amount");
                return;
            }

            Value += amount;
        }

        public bool IsDepleted => Mathf.Approximately(Value, 0);
    }
}