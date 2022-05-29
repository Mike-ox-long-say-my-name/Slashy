using System;

namespace Core
{
}

namespace Core
{
    public interface IGameLoader
    {
        event SceneCallback Exiting;
        event SceneCallback LoadingLevel;
        event SceneCallback StartingNewGame;
        event SceneCallback LoadedLevel;
        event SceneCallback LoadedExitedLevel;
        event SceneCallback LoadingExitedLevel;
        event Action GameCompleted;
        bool HasAnyGameProgress { get; }
        void LoadLevel(string levelName);
        void LoadGame();
        void LoadNewGame();
        void LoadMenu();
        void CompleteGame();
        event Action Exited;
    }
}