using System;
using Object = UnityEngine.Object;

namespace Core.Characters
{
    public static class Guard
    {
        public static void NotNull(Object unityObject)
        {
            if (unityObject == null)
            {
                throw new ArgumentNullException(nameof(unityObject));
            }
        }

        public static void NotNull(object nativeObject)
        {
            if (nativeObject == null)
            {
                throw new ArgumentNullException(nameof(nativeObject));
            }
        }
    }
}