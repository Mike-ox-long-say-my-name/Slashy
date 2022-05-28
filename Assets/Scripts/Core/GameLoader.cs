using System;
using Core.Levels;
using UnityEngine;

namespace Core
{
    public class GameLoader : IGameLoader
    {
        private readonly RespawnData _respawnData;
        private readonly BonfireSaveData _bonfireData;

        private const string NewGameScene = "Temple"; 
        private const string MenuScene = "MenuScene";

        public event SceneCallback Exiting;
        public event SceneCallback LoadingLevel;
        public event SceneCallback StartingNewGame;
        public event SceneCallback LoadedLevel;
        public event SceneCallback LoadedExitedLevel;
        public event SceneCallback LoadingExitedLevel;
        public event Action GameCompleted;

        public bool HasAnyGameProgress => !string.IsNullOrEmpty(_respawnData.RespawnLevel);

        private readonly ISceneLoader _sceneLoader;

        public GameLoader(ISceneLoader sceneLoader, SaveDataContainer saveDataContainer)
        {
            _sceneLoader = sceneLoader;
            _respawnData = saveDataContainer.RespawnData;
            _bonfireData = saveDataContainer.BonfireSaveData;
        }

        public void LoadLevel(string levelName)
        {
            LoadingLevel?.Invoke(levelName);
            _sceneLoader.ReplaceLastScene(levelName);
            LoadedLevel?.Invoke(levelName);
        }

        public void LoadGame()
        {
            var exitedLevel = _respawnData.RespawnLevel;
            
            LoadingExitedLevel?.Invoke(exitedLevel);
            _sceneLoader.ReplaceLastScene(exitedLevel);
            LoadedExitedLevel?.Invoke(exitedLevel);
        }

        public void LoadNewGame()
        {
            _bonfireData.ResetBitmask();
            _respawnData.ResetData();

            StartingNewGame?.Invoke(NewGameScene);
            LoadLevel(NewGameScene);
        }

        public void LoadMenu()
        {
            Exiting?.Invoke(_sceneLoader.CurrentSceneName);
            _sceneLoader.ReplaceLastScene(MenuScene);
        }

        public void CompleteGame()
        {
            GameCompleted?.Invoke();
            
            _bonfireData.ResetBitmask();
            _respawnData.ResetData();
            
            LoadMenu();
        }
    }
}