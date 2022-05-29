using System;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoader : ISceneLoader
    {
        private const string ScenesPath = "Scenes";

        public event Action<string> SceneLoaded;
        public event Action<string> SceneUnloading;
        public event Action<string> SceneUnloaded;

        public string CurrentSceneName { get; private set; }

        public void SetActive(string name)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        }

        public void LoadScene(string name)
        {
            CurrentSceneName = name;
            var scene = SceneManager.GetSceneByName(name);
            if (scene.isLoaded)
            {
                SceneLoaded?.Invoke(name);
                return;
            }
            
            var operation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            operation.completed += _ => SceneLoaded?.Invoke(name);
        }

        public void UnloadScene(string name)
        {
            SceneUnloading?.Invoke(name);
            var operation = SceneManager.UnloadSceneAsync(name);
            operation.completed += _ => SceneUnloaded?.Invoke(name);
        }

        public void ReplaceLastScene(string name)
        {
            UnloadScene(CurrentSceneName);
            LoadScene(name);
        }
    }
}