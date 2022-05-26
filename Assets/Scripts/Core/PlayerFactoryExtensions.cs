namespace Core
{
    public static class PlayerFactoryExtensions
    {
        public static LazyPlayer GetLazy(this IPlayerFactory playerFactory)
        {
            return new LazyPlayer(playerFactory);
        }
    }
}