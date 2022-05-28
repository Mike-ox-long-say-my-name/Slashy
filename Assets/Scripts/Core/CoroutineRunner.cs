using System;
using System.Collections;
using UnityEngine;

namespace Core
{
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