using System.Collections;
using UnityEngine;

namespace Core
{
    public class CoroutineHost : ICoroutineHost
    {
        private readonly MonoBehaviour _monoBehaviour;

        public GameObject Object => _monoBehaviour.gameObject;

        public CoroutineHost(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }

        public Coroutine Start(IEnumerator enumerator)
        {
            return _monoBehaviour.StartCoroutine(enumerator);
        }

        public void Stop(Coroutine coroutine)
        {
            _monoBehaviour.StopCoroutine(coroutine);
        }
    }
}