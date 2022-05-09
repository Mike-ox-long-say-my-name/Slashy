using System;
using Object = UnityEngine.Object;

namespace Core.Utilities
{
    public static class Guard
    {
        public static void NotNull(Object unityObject, string message = null)
        {
            if (unityObject == null)
            {
                throw new ArgumentNullException(nameof(unityObject), message);
            }
        }

        public static void NotNull(object nativeObject, string message = null)
        {
            if (nativeObject == null)
            {
                throw new ArgumentNullException(nameof(nativeObject), message);
            }
        }
    }
}