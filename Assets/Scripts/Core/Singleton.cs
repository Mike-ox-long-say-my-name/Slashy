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
                    Debug.LogWarning($"Creating singleton {GetName()} automatically");
                }
                return _instance;
            }
            set => _instance = value;
        }

        protected static T TryGetInstance()
        {
            return _instance;
        }

        private static string GetName()
        {
            return $"Singleton - {typeof(T).Name}";
        }

        private static void CreateInstance()
        {
            var instanceObject = new GameObject(GetName())
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