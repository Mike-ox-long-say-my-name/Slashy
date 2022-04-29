namespace Core
{
    public interface IGameLoader
    {
        bool HasAnyGameProgress { get; }

        void LoadGame();
        void LoadNewGame();

        void LoadMenu();
    }
}