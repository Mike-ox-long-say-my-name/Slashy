using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameLoader : PublicSingleton<GameLoader>, IGameLoader
    {
        private const string GameSceneName = "Scenes/SampleScene";
        private const string MenuSceneName = "Scenes/MenuScene";

        public bool HasAnyGameProgress => false;

        public void LoadGame()
        {
            Debug.LogWarning("Not implemented", this);
        }

        public void LoadNewGame()
        {
            TryLoadScene(GameSceneName);
        }

        public void LoadMenu()
        {
            TryLoadScene(MenuSceneName);
        }

        private void TryLoadScene(string sceneName)
        {
            var sceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
            if (sceneIndex == -1)
            {
                Debug.LogWarning("Scene is missing", this);
                return;
            }

            SceneManager.LoadScene(sceneIndex);
        }
    }
}