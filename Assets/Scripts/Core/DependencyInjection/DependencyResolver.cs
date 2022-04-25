using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.DependencyInjection
{
    [DefaultExecutionOrder(-400)]
    public class DependencyResolver : Singleton<DependencyResolver>
    {
        protected override void Awake()
        {
            base.Awake();

            int resolved = 0;
            var monoBehaviours = FindObjectsOfType<MonoBehaviour>();
            foreach (var monoBehaviour in monoBehaviours)
            {
                resolved += ResolveInternal(monoBehaviour);
            }

            print($"Resolved: {resolved}");
        }

        private static int ResolveInternal(Object unityObject)
        {
            int resolved = 0;
            var resolve = unityObject.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(method => method.GetCustomAttribute<AutoResolveAttribute>() != null);

            try
            {
                foreach (var method in resolve)
                {
                    method.Invoke(unityObject, null);
                    resolved += 1;
                }
            }
            catch (Exception)
            {
                Debug.LogWarning($"Exception occurred while resolving {unityObject.name}.");
                throw;
            }


            return resolved;
        }

        private static int ResolveInternal(GameObject gameObject)
        {
            int resolved = 0;
            foreach (var mnoBehaviour in gameObject.GetComponentsInChildren<MonoBehaviour>())
            {
                resolved += ResolveInternal(mnoBehaviour);
            }

            return resolved;
        }

        public static void Resolve(GameObject gameObject)
        {
            ResolveInternal(gameObject);
            print($"Resolved {gameObject.name}");
        }
    }
}