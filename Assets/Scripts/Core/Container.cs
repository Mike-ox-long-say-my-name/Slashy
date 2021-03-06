using System;
using System.Collections.Generic;

namespace Core
{
    public static class Container
    {
        private class ServiceDescription
        {
            private readonly Func<object> _serviceCtor;
            private readonly ServiceLifetime _lifetime;

            private object _service;
            private bool _isDirty;

            public ServiceDescription(Func<object> serviceCtor, ServiceLifetime lifetime)
            {
                _serviceCtor = serviceCtor;
                _lifetime = lifetime;
            }

            public T GetService<T>()
            {
                switch (_lifetime)
                {
                    case ServiceLifetime.PerScene:
                        if (_isDirty || _service == null)
                        {
                            _service = _serviceCtor();
                            _isDirty = false;
                        }

                        return (T)_service;
                    case ServiceLifetime.Singleton:
                        _service ??= _serviceCtor();

                        return (T)_service;
                    case ServiceLifetime.PerObject:
                        return (T)_serviceCtor();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public void SetDirtyIfPerScene()
            {
                _isDirty = _lifetime == ServiceLifetime.PerScene;
            }
        }

        static Container()
        {
            InitializeSceneLoader();
        }

        private static void InitializeSceneLoader()
        {
            Add<ISceneLoader, SceneLoader>(ServiceLifetime.Singleton);
            var sceneLoader = Get<ISceneLoader>();
            sceneLoader.SceneUnloaded += _ => SetPerSceneServicesDirty();
        }

        private static void SetPerSceneServicesDirty()
        {
            foreach (var serviceType in Services.Keys)
            {
                Services[serviceType].SetDirtyIfPerScene();
            }
        }

        private static readonly Dictionary<Type, ServiceDescription> Services =
            new Dictionary<Type, ServiceDescription>();

        public static void AddSingletonInstance<T>(T instance) where T : class
        {
            Services[typeof(T)] = new ServiceDescription(() => instance, ServiceLifetime.Singleton);
        }

        public static void AddSingletonInstance<TInterface, TClass>(TClass instance) where TClass : class, TInterface
        {
            Services[typeof(TInterface)] = new ServiceDescription(() => instance, ServiceLifetime.Singleton);
        }

        public static void Add<T>(ServiceLifetime lifetime = ServiceLifetime.PerObject) where T : class, new()
        {
            Add(() => new T(), lifetime);
        }

        public static void Add<TInterface, TClass>(ServiceLifetime lifetime = ServiceLifetime.PerObject)
            where TClass : class, TInterface, new()
            where TInterface : class
        {
            Add<TInterface>(() => new TClass(), lifetime);
        }

        public static void Add<T>(Func<T> ctor, ServiceLifetime lifetime = ServiceLifetime.PerObject)
            where T : class
        {
            Services[typeof(T)] = new ServiceDescription(ctor, lifetime);
        }

        public static T Get<T>() where T : class
        {
            if (!Services.TryGetValue(typeof(T), out var serviceDesc))
            {
                throw new NullReferenceException($"Type {typeof(T)} has not been registered.");
            }

            return serviceDesc.GetService<T>();
        }
    }
}