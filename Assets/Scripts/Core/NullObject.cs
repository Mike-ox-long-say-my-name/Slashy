using UnityEngine;

namespace Core
{
    public abstract class NullObject<T>
    {
        private readonly string _name = typeof(T).Name;
        private readonly Object _context;

        protected NullObject(Object context)
        {
            _context = context;
            Report();
        }

        protected void Report()
        {
            Debug.LogWarning($"{_name} is missing", _context);
        }

        protected static TDummy Dummy<TDummy>() where TDummy : Component
        {
            return DummyObjectFactory.Get<TDummy>();
        }
    }
}