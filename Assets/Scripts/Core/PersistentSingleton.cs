using UnityEngine;

namespace Core
{
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    CreateInstance();
                }
                return _instance;
            }
        }

        private static void CreateInstance()
        {
            var instanceObject = new GameObject($"PersistentSingleton - {typeof(T).Name}")
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            _instance = instanceObject.AddComponent<T>();
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}