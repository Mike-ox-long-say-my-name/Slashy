using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoader
    {
        private const string ScenesPath = "Scenes";

        public event Action<string> SceneLoaded;

        public string CurrentSceneName { get; private set; }

        public static void SetActive(string name)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        }

        public void LoadScene(string name)
        {
            CurrentSceneName = name;

            var path = GetScenePath(name);
            var scene = SceneManager.GetSceneByName(name);
            if (scene.isLoaded)
            {
                return;
            }
            SceneManager.LoadScene(path, LoadSceneMode.Additive);
            SceneLoaded?.Invoke(name);
        }

        private string GetScenePath(string name)
        {
            return Path.Combine(ScenesPath, name);
        }

        public void UnloadScene(string name)
        {
            SceneManager.UnloadSceneAsync(name);
        }

        public void ReplaceLastScene(string name)
        {
            UnloadScene(CurrentSceneName);
            LoadScene(name);
        }
    }
}