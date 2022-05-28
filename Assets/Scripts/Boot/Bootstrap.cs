using Characters.Enemies.Boss;
using Core.Levels;
using Settings;
using UI.PopupHints;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-100)]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private string afterLoadScene;

        private ISceneLoader _sceneLoader;

        private void Awake()
        {
            Initialize();

            _sceneLoader = Container.Get<ISceneLoader>();
            _sceneLoader.SceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            _sceneLoader.LoadScene(afterLoadScene);
        }

        private void OnSceneLoaded(string scene)
        {
            _sceneLoader.SetActive(scene);
        }

        private static void Initialize()
        {
            AddMonoBehaviourSingletons();

            BindSingletonServices();
            BindPerSceneServices();
        }

        private static void BindSingletonServices()
        {
            BindGameLoader();

            BindPlayerFactory();
            BindPlayerBindings();
            BindBackgroundMusicPlayer();
            BindVolumeControlService();

            BindBlackScreenService();
            BindPlayerDeathSequencePlayer();
            BindGameEndedSequencePlayer();

            BindRespawnController();
            BindLevelWarper();
        }

        private static void AddMonoBehaviourSingletons()
        {
            AddObjectLocator();
            AddTimerRunner();
            AddCoroutineRunner();
            AddBackgroundMusicContainer();
            AddVolumeControlContainer();
            AddBlackScreenServiceContainer();
            AddSaveDataContainer();
        }

        private static void AddPopupHintContainer()
        {
            Container.Add(FindObjectOfType<PopupHintContainer>, ServiceLifetime.PerScene);
        }

        private static void BindLevelWarper()
        {
            Container.Add<ILevelWarper>(() =>
            {
                var coroutineRunner = Container.Get<ICoroutineRunner>();
                var gameLoader = Container.Get<IGameLoader>();
                var blackScreenService = Container.Get<IBlackScreenService>();
                var playerFactory = Container.Get<IPlayerFactory>();
                return new LevelWarper(coroutineRunner, gameLoader, blackScreenService, playerFactory);
            }, ServiceLifetime.Singleton);
        }

        private static void BindPlayerBindings()
        {
            Container.Add<PlayerBindings>(ServiceLifetime.Singleton);
        }

        private static void BindGameEndedSequencePlayer()
        {
            Container.Add<IGameEndedSequencePlayer>(() =>
            {
                var coroutineRunner = Container.Get<ICoroutineRunner>();
                var blackScreenService = Container.Get<IBlackScreenService>();
                return new GameEndedSequencePlayer(coroutineRunner, blackScreenService);
            }, ServiceLifetime.Singleton);
        }

        private static void AddSaveDataContainer()
        {
            AddSingletonFromScene<SaveDataContainer>();
        }

        private static void AddBlackScreenServiceContainer()
        {
            AddSingletonFromScene<BlackScreenServiceContainer>();
        }

        private static void AddObjectLocator()
        {
            AddSingletonFromScene<IObjectLocator, ObjectLocator>();
        }

        private static void BindRespawnController()
        {
            Container.Add<IRespawnController>(() =>
            {
                var sceneLoader = Container.Get<ISceneLoader>();
                var gameLoader = Container.Get<IGameLoader>();
                var deathSequencePlayer = Container.Get<IPlayerDeathSequencePlayer>();
                var playerFactory = Container.Get<IPlayerFactory>();
                var saveDataContainer = Container.Get<SaveDataContainer>();
                return new RespawnController(sceneLoader, gameLoader, deathSequencePlayer,
                    playerFactory, saveDataContainer);
            }, ServiceLifetime.Singleton);
        }

        private static void BindPlayerDeathSequencePlayer()
        {
            Container.Add<IPlayerDeathSequencePlayer>(() =>
            {
                var coroutineRunner = Container.Get<ICoroutineRunner>();
                var blackScreenService = Container.Get<IBlackScreenService>();
                var gameLoader = Container.Get<IGameLoader>();
                return new PlayerDeathSequencePlayer(coroutineRunner, blackScreenService, gameLoader);
            }, ServiceLifetime.Singleton);
        }

        private static void BindBlackScreenService()
        {
            Container.Add<IBlackScreenService>(() =>
            {
                var coroutineRunner = Container.Get<ICoroutineRunner>();
                var gameLoader = Container.Get<IGameLoader>();
                var container = Container.Get<BlackScreenServiceContainer>();
                return new BlackScreenService(coroutineRunner, gameLoader, container);
            }, ServiceLifetime.Singleton);
        }

        private static void AddVolumeControlContainer()
        {
            AddSingletonFromScene<VolumeControlServiceContainer>();
        }

        private static void BindVolumeControlService()
        {
            Container.Add<IVolumeControlService>(() =>
            {
                var container = Container.Get<VolumeControlServiceContainer>();
                return new VolumeControlService(container);
            }, ServiceLifetime.Singleton);
        }

        private static void BindBackgroundMusicPlayer()
        {
            Container.Add<IBackgroundMusicPlayer>(() =>
            {
                var container = Container.Get<BackgroundMusicAudioSourceContainer>();
                var aggroListener = Container.Get<IAggroListener>();
                return new BackgroundMusicPlayer(container, aggroListener);
            }, ServiceLifetime.Singleton);
        }

        private static void AddBackgroundMusicContainer()
        {
            AddSingletonFromScene<BackgroundMusicAudioSourceContainer>();
        }

        private static void BindPerSceneServices()
        {
            BindPopupHintController();
            BindAggroListener();
            BindEnemyFactory();
            BindUiFactory();
            BindInteractionService();
            BindBorderService();
            BindAmbushResetter();
            
            AddPopupHintContainer();
        }

        private static void BindAmbushResetter()
        {
            Container.Add<IAmbushResetter>(() =>
            {
                var objectLocator = Container.Get<IObjectLocator>();
                return new AmbushResetter(objectLocator);
            }, ServiceLifetime.PerScene);
        }

        private static void BindPopupHintController()
        {
            Container.Add<IPopupHintController>(() =>
            {
                var hintContainer = Container.Get<PopupHintContainer>();
                return new PopupHintController(hintContainer);
            }, ServiceLifetime.PerScene);
        }

        private static void BindUiFactory()
        {
            Container.Add<IUiFactory, UiFactory>(ServiceLifetime.PerScene);
        }

        private static void BindEnemyFactory()
        {
            Container.Add<IEnemyFactory>(() =>
            {
                var objectLocator = Container.Get<IObjectLocator>();
                return new EnemyFactory(objectLocator);
            }, ServiceLifetime.PerScene);
        }

        private static void BindBorderService()
        {
            Container.Add<IBorderService>(() =>
            {
                var objectLocator = Container.Get<IObjectLocator>();
                var aggroListener = Container.Get<IAggroListener>();
                var playerFactory = Container.Get<IPlayerFactory>();
                return new BorderService(objectLocator, aggroListener, playerFactory);
            }, ServiceLifetime.PerScene);
        }

        private static void BindInteractionService()
        {
            Container.Add<IInteractionService>(() =>
            {
                var objectLocator = Container.Get<IObjectLocator>();
                return new InteractionService(objectLocator);
            }, ServiceLifetime.PerScene);
        }

        private static void BindAggroListener()
        {
            Container.Add<IAggroListener>(() =>
            {
                var sceneLoader = Container.Get<ISceneLoader>();
                return new AggroListener(sceneLoader);
            }, ServiceLifetime.Singleton);
        }

        private static void BindPlayerFactory()
        {
            Container.Add<IPlayerFactory>(() =>
            {
                var objectLocator = Container.Get<IObjectLocator>();
                var sceneLoader = Container.Get<ISceneLoader>();
                return new PlayerFactory(objectLocator, sceneLoader);
            }, ServiceLifetime.Singleton);
        }

        private static void BindGameLoader()
        {
            Container.Add<IGameLoader>(() =>
            {
                var sceneLoader = Container.Get<ISceneLoader>();
                var saveDataContainer = Container.Get<SaveDataContainer>();
                return new GameLoader(sceneLoader, saveDataContainer);
            }, ServiceLifetime.Singleton);
        }

        private static void AddCoroutineRunner() => AddSingletonFromScene<ICoroutineRunner, CoroutineRunner>();

        private static void AddTimerRunner() => AddSingletonFromScene<ITimerRunner, TimerRunner>();

        private static void AddSingletonFromScene<TMonoBehaviour>() where TMonoBehaviour : MonoBehaviour
        {
            var monoBehaviour = FindObjectOfType<TMonoBehaviour>();
            if (!monoBehaviour)
            {
                Debug.LogError($"Cannot find {typeof(TMonoBehaviour).Name}");
                return;
            }

            Container.AddSingletonInstance(monoBehaviour);
        }

        private static void AddSingletonFromScene<TInterface, TMonoBehaviour>()
            where TMonoBehaviour : MonoBehaviour, TInterface
        {
            var monoBehaviour = FindObjectOfType<TMonoBehaviour>();
            if (!monoBehaviour)
            {
                Debug.LogError($"Cannot find {typeof(TMonoBehaviour).Name}");
                return;
            }

            Container.AddSingletonInstance<TInterface, TMonoBehaviour>(monoBehaviour);
        }
    }
}