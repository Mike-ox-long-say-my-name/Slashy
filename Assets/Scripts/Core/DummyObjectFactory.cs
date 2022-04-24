using UnityEngine;

namespace Core
{
    public static class DummyObjectFactory
    {
        private static GameObject _gameObject;

        public static T Get<T>() where T : Component
        {
            if (_gameObject == null)
            {
                _gameObject = new GameObject
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }
            return _gameObject.TryGetComponent<T>(out var component) ? component : _gameObject.AddComponent<T>();
        }
    }
}