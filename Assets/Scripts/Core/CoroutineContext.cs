using System.Collections;
using UnityEngine;

namespace Core
{
    public class CoroutineContext
    {
        private readonly CoroutineRunner _runner;
        private Coroutine _coroutine;

        public CoroutineContext(CoroutineRunner runner, Coroutine coroutine = null)
        {
            _coroutine = coroutine;
            _runner = runner;
        }

        public void Restart(IEnumerator enumerator)
        {
            Stop();
            Start(enumerator);
        }
        
        public void Start(IEnumerator enumerator)
        {
            _coroutine = _runner.Run(enumerator);
        }

        public void Stop()
        {
            if (_coroutine == null)
            {
                return;
            }
            
            _runner.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        public static implicit operator Coroutine(CoroutineContext context)
        {
            return context._coroutine;
        }
    }
}