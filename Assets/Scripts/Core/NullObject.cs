using UnityEngine;

namespace Core
{
    public class NullObject<T>
    {
        private readonly string _name = typeof(T).Name;

        protected void Report()
        {
            Debug.LogWarning($"{_name} is missing");
        }
    }
}