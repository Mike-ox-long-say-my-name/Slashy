using UnityEngine;

namespace Core.Utilities
{
    public static class CoroutineHostConverter
    {
        public static ICoroutineHost ToCoroutineHost(this MonoBehaviour monoBehaviour)
        {
            return new CoroutineHost(monoBehaviour);
        }
    }
}