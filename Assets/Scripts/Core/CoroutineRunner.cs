using System;
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

    public interface ICoroutineRunner
    {
        CoroutineContext RunAfter(Action action, float time);
        CoroutineContext RunAfter(Action action, Coroutine previousCoroutine);
        CoroutineContext GetEmptyContext();
    }

    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public CoroutineContext GetEmptyContext()
        {
            return new CoroutineContext(this);
        }

        public CoroutineContext Run(IEnumerator enumerator)
        {
            var coroutine = StartCoroutine(enumerator);
            return new CoroutineContext(this, coroutine);
        }
        
        public CoroutineContext RunAfter(Action action, float time)
        {
            var coroutine = StartCoroutine(RunAfterRoutine(action, time));
            return new CoroutineContext(this, coroutine);
        }

        public CoroutineContext RunAfter(Action action, Coroutine previousCoroutine)
        {
            var coroutine = StartCoroutine(RunAfterRoutine(action, previousCoroutine));
            return new CoroutineContext(this, coroutine);
        }

        private static IEnumerator RunAfterRoutine(Action action, float time)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        private static IEnumerator RunAfterRoutine(Action action, Coroutine coroutine)
        {
            yield return coroutine;
            action();
        }
    }
}