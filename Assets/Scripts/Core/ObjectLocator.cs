using System.Linq;
using UnityEngine;

namespace Core
{
    public class ObjectLocator : MonoBehaviour, IObjectLocator
    {
        public T[] FindAll<T>()
        {
            return FindObjectsOfType<MonoBehaviour>().OfType<T>().ToArray();
        }
    }
}