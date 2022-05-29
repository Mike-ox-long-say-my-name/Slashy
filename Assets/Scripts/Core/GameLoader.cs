using System;
using Core.Levels;
using UI.PopupHints;
using UnityEngine;

namespace Core
{
    public class GameLoader : IGameLoader
    {
        private readonly RespawnData _respawnData;
        private readonly BonfireSaveData _bonfireData;
        private readonly ShownHintsSO _shownHints;

        private static readonly string NewGameScene = LevelLibrary.GetLevel(0);
        private const string MenuScene = LevelLibrary.Menu;

        private const string GameStartedPrefName = "GameStarted";

        public event SceneCallback Exiting;
        public event SceneCallback LoadingLevel;
        public event SceneCallback StartingNewGame;
        public event SceneCallback LoadedLevel;
        public event SceneCallback LoadedExitedLevel;
        public event SceneCallback LoadingExitedLevel;
        public event Action GameCompleted;
        public event Action Exited;

        public bool HasAnyGameProgress => PlayerPrefs.GetInt(GameStartedPrefName, 0) == 1;

        private readonly ISceneLoader _sceneLoader;

        public GameLoader(ISceneLoader sceneLoader, SaveDataContainer saveDataContainer)
        {
            _sceneLoader = sceneLoader;
            _respawnData = saveDataContainer.RespawnData;
            _bonfireData = saveDataContainer.BonfireSaveData;
            _shownHints = saveDataContainer.ShownHints;
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
            PlayerPrefs.SetInt(GameStartedPrefName, 1);

            ResetSavedData();
            
            StartingNewGame?.Invoke(NewGameScene);
            LoadLevel(NewGameScene);
        }

        private void ResetSavedData()
        {
            _bonfireData.ResetBitmask();
            _respawnData.ResetData();
            _shownHints.ResetShownHints();
        }

        public void LoadMenu()
        {
            Exiting?.Invoke(_sceneLoader.CurrentSceneName);
            _sceneLoader.ReplaceLastScene(MenuScene);
            Exited?.Invoke();
        }

        public void CompleteGame()
        {
            GameCompleted?.Invoke();

            PlayerPrefs.SetInt(GameStartedPrefName, 0);
            ResetSavedData();
            LoadMenu();
        }
    }
}