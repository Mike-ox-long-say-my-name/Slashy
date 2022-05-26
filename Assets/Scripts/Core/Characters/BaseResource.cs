using System;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters
{
    public abstract class BaseResource : IResource
    {
        public void ForceRaiseEvent()
        {
            ValueChanged?.Invoke(this);
        }

        public event Action<IResource> ValueChanged;

        private float _value;

        public bool Frozen { get; set; } = false;
        public float MaxValue { get; }

        public float Value
        {
            get => _value;
            set
            {
                if (Frozen)
                {
                    return;
                }

                var newValue = Mathf.Clamp(value, 0, MaxValue);
                if (Mathf.Approximately(newValue, _value))
                {
                    return;
                }

                _value = newValue;
                ValueChanged?.Invoke(this);
            }
        }

        private BaseResource(float maxValue, float startValue)
        {
            MaxValue = maxValue;
            Value = startValue;
        }

        protected BaseResource(float maxValue) : this(maxValue, maxValue)
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
    }
}