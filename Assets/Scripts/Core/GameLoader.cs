using System;
using Core.Levels;
using UnityEngine;

namespace Core
{
    public class GameLoader : IGameLoader
    {
        [SerializeField] private RespawnData respawnData;
        [SerializeField] private BonfireSaveData bonfireData;

        private const string NewGameScene = "Temple"; 
        private const string MenuScene = "MenuScene";

        public event SceneCallback Exiting;
        public event SceneCallback LoadingLevel;
        public event SceneCallback StartingNewGame;
        public event SceneCallback LoadedLevel;
        public event SceneCallback LoadedExitedLevel;
        public event SceneCallback LoadingExitedLevel;
        public event Action GameCompleted;

        public bool HasAnyGameProgress => !string.IsNullOrEmpty(respawnData.RespawnLevel);

        private readonly SceneLoader _sceneLoader;

        public GameLoader()
        {
            _sceneLoader = Container.Get<SceneLoader>();
        }

        public void LoadLevel(string levelName)
        {
            LoadingLevel?.Invoke(levelName);
            _sceneLoader.LoadScene(levelName);
            LoadedLevel?.Invoke(levelName);
        }

        public void LoadGame()
        {
            var exitedLevel = respawnData.RespawnLevel;
            
            LoadingExitedLevel?.Invoke(exitedLevel);
            _sceneLoader.LoadScene(exitedLevel);
            LoadedExitedLevel?.Invoke(exitedLevel);
        }

        public void LoadNewGame()
        {
            bonfireData.ResetBitmask();

            StartingNewGame?.Invoke(NewGameScene);
            LoadLevel(NewGameScene);
        }

        public void LoadMenu()
        {
            Exiting?.Invoke(_sceneLoader.CurrentSceneName);
            _sceneLoader.LoadScene(MenuScene);
        }

        public void CompleteGame()
        {
            GameCompleted?.Invoke();
            bonfireData.ResetBitmask();
            LoadMenu();
        }
    }
}