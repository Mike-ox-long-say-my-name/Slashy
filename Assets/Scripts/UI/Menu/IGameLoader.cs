namespace UI.Menu
{
    public interface IGameLoader
    {
        bool HasAnyGameProgress { get; }

        void LoadGame();
        void LoadNewGame();

        void LoadMenu();
    }
}