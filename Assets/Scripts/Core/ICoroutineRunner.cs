using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public interface ICoroutineRunner
    {
        CoroutineContext RunAfter(Action action, float time);
        CoroutineContext RunAfter(Action action, Coroutine previousCoroutine);
        CoroutineContext GetEmptyContext();
        CoroutineContext Run(IEnumerator enumerator);
    }
}