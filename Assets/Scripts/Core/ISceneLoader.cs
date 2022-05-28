using System;

namespace Core
{
    public interface ISceneLoader
    {
        event Action<string> SceneLoaded;
        event Action<string> SceneUnloading;
        event Action<string> SceneUnloaded;
        string CurrentSceneName { get; }
        void LoadScene(string name);
        void UnloadScene(string name);
        void ReplaceLastScene(string name);
        void SetActive(string name);
    }
}