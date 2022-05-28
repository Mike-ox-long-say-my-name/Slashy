namespace Core
{
    public static class PlayerFactoryExtensions
    {
        public static LazyPlayer GetLazyPlayer(this IPlayerFactory playerFactory)
        {
            return new LazyPlayer(playerFactory);
        }
    }
}