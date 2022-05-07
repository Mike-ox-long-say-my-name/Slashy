using System;
using UnityEngine;

namespace Core.Characters.Interfaces
{
    public interface IResource
    {
        float MaxValue { get; }
        float Value { get; set; }

        void Recover(float amount);
        void Spend(float amount);

        event Action<IResource> OnValueChanged;
    }

    public static class CharacterResourceExtensions
    {
        public static bool IsDepleted(this IResource resource)
        {
            return Mathf.Approximately(resource.Value, 0);
        }

        public static bool HasAny(this IResource resource)
        {
            return resource.Value > 0;
        }
    }
}