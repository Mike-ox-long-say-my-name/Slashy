using UnityEngine;

namespace Core
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        protected static T Instance
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
            var instanceObject = new GameObject($"Singleton - {typeof(T).Name}")
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
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}