using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameLoader : PublicSingleton<GameLoader>, IGameLoader
    {
        [SerializeField] private SceneAsset gameScene;
        [SerializeField] private SceneAsset menuScene;

        public bool HasAnyGameProgress => false;

        public void LoadGame()
        {
            Debug.LogWarning("Not implemented", this);
        }

        public void LoadNewGame()
        {
            TryLoadScene("Scenes/Levels/Temple");
        }

        public void LoadMenu()
        {
            TryLoadScene("Scenes/MenuScene");
        }

        private void TryLoadScene(string asset)
        {
            var sceneName = asset;
            var sceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
            if (sceneIndex == -1)
            {
                Debug.LogWarning("Scene is missing in build info", this);
                return;
            }

            SceneManager.LoadScene(sceneIndex);
        }
    }
}