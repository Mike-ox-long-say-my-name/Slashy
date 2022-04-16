using System;
using UnityEngine;

public abstract class BaseCharacterResource : ICharacterResource
{
    public Character Character { get; }

    public float MaxValue { get; }
    public float Value { get; protected set; }
    
    protected BaseCharacterResource(Character character, float maxValue, float startValue)
    {
        Character = character;
        MaxValue = maxValue;
        Value = startValue;
    }

    protected BaseCharacterResource(Character character, float maxValue) : this(character, maxValue, maxValue)
    {
    }

    public virtual void Spend(float amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException();
        }
        Value = Mathf.Max(0, Value - amount);
    }

    public virtual void Recover(float amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException();
        }
        Value = Mathf.Min(MaxValue, Value + amount);
    }

    public bool IsDepleted => Mathf.Approximately(Value, 0);
}