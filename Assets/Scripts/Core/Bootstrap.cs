using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-100)]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private string afterLoadScene;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            var sceneLoader = Container.Get<SceneLoader>();
            sceneLoader.LoadScene(afterLoadScene);

            SceneLoader.SetActive(afterLoadScene);
        }

        private void Initialize()
        {
            BindGameLoader();
            
            AddTimerRunner();
            AddCoroutineRunner();
        }

        private void BindGameLoader()
        {
            Container.Add<IGameLoader, GameLoader>();
        }

        private void AddCoroutineRunner() => AddSingletonFromScene<CoroutineRunner>();

        private void AddTimerRunner() => AddSingletonFromScene<TimerRunner>();

        private void AddSingletonFromScene<TMonoBehaviour>() where TMonoBehaviour : MonoBehaviour
        {
            var monoBehaviour = FindObjectOfType<TMonoBehaviour>();
            if (!monoBehaviour)
            {
                Debug.LogError($"Cannot find {nameof(TMonoBehaviour)}");
                return;
            }

            Container.AddSingletonInstance(monoBehaviour);
        }
    }
}