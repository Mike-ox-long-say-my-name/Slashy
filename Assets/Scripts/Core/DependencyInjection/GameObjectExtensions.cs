using UnityEngine;

namespace Core.DependencyInjection
{
    public static class GameObjectExtensions
    {
        public static GameObject Resolve(this GameObject gameObject)
        {
            DependencyResolver.Resolve(gameObject);
            return gameObject;
        }
    }
}