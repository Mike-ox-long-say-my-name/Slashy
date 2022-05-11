using UnityEngine;

namespace Core
{
    public class PublicSingleton<T> : Singleton<T> where T : Component
    {
        public new static T Instance => Singleton<T>.Instance;

        public new static T TryGetInstance()
        {
            return Singleton<T>.TryGetInstance();
        }
    }
}