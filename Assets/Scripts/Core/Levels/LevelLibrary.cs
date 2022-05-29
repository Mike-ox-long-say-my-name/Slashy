namespace Core.Levels
{
    public static class LevelLibrary
    {
        public const string Menu = "Menu";

        private static readonly string[] Levels =
        {
            "Temple",
            "Forest",
            "BossArena"
        };

        public static string GetLevel(int index) => Levels[index];
        public static int LevelCount => Levels.Length;
    }
}