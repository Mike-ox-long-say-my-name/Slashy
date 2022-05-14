using Core.Levels;
using Core.Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameLoader : PublicSingleton<GameLoader>
    {
        [SerializeField] private SaveData saveData;

        [SerializeField] private UnityEvent<string> exiting;
        [SerializeField] private UnityEvent<string> loadingLevel;
        [SerializeField] private UnityEvent<string> loadingExitedLevel;
        [SerializeField] private UnityEvent<string> startingNewGame;
        [SerializeField] private UnityEvent<string> loadedLevel;
        [SerializeField] private UnityEvent<string> loadedExitedLevel;

        [SerializeField] private SceneAsset newGameScene;
        [SerializeField] private SceneAsset menuScene;

        public UnityEvent<string> Exiting => exiting;

        public UnityEvent<string> LoadingLevel => loadingLevel;

        public UnityEvent<string> StartingNewGame => startingNewGame;
        public UnityEvent<string> LoadedLevel => loadedLevel;
        public UnityEvent<string> LoadedExitedLevel => loadedExitedLevel;
        public UnityEvent<string> LoadingExitedLevel => loadingExitedLevel;

        public bool HasAnyGameProgress => !string.IsNullOrEmpty(saveData.RespawnData.RespawnLevel);

        private string _scheduledScene;

        protected override void Awake()
        {
            base.Awake();
            MapOtherSceneToSetActiveLater();
        }

        private void MapOtherSceneToSetActiveLater()
        {
            var managersScene = GetCurrentScene();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == managersScene)
                {
                    continue;
                }

                _scheduledScene = scene.name;
                SceneManager.sceneLoaded += SetActiveLater;
                return;
            }
        }

        private void SetActiveLater(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != _scheduledScene)
            {
                return;
            }

            SceneManager.sceneLoaded -= SetActiveLater;
            _scheduledScene = null;
            SceneManager.SetActiveScene(scene);
        }

        public void LoadLevel(string levelName)
        {
            LoadingLevel?.Invoke(levelName);
            TryLoadScene(levelName);
            LoadedLevel?.Invoke(levelName);
            PlayerManager.Instance.ActivatePlayer();
        }

        public void LoadGame()
        {
            var exitedLevel = saveData.RespawnData.RespawnLevel;
            LoadingExitedLevel?.Invoke(exitedLevel);
            TryLoadScene(exitedLevel);
            LoadedExitedLevel?.Invoke(exitedLevel);
        }

        public void LoadNewGame()
        {
            var levelName = newGameScene.name;
            StartingNewGame?.Invoke(levelName);
            LoadLevel(levelName);
        }

        public void LoadMenu()
        {
            Exiting?.Invoke(GetCurrentScene());
            TryLoadScene(menuScene.name);
        }

        private void TryLoadScene(string sceneName)
        {
            var sceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
            if (sceneIndex == -1)
            {
                Debug.LogWarning($"Scene {sceneName} is missing in build info", this);
                return;
            }

            var currentScene = GetCurrentScene();
            if (currentScene != null)
            {
                SceneManager.UnloadSceneAsync(currentScene);
            }
            var operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            operation.completed += _ => SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
        }

        public static string GetCurrentScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            return activeScene.IsValid() ? activeScene.name : null;
        }
    }
}