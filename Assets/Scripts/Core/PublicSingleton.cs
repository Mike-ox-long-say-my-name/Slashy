using UnityEngine;

namespace Core
{
    public class PublicSingleton<T> : Singleton<T> where T : Component
    {
        public new static T Instance => Singleton<T>.Instance;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
    }
}