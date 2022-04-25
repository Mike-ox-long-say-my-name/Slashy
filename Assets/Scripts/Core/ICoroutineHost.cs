using System.Collections;
using UnityEngine;

namespace Core
{
    public interface ICoroutineHost
    {
        GameObject Object { get; }

        Coroutine Start(IEnumerator enumerator);
        void Stop(Coroutine coroutine);
    }
}