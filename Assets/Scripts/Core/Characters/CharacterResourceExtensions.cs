using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters
{
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