using Core.Player.Interfaces;

namespace Core
{
    public class LazyPlayer
    {
        public bool IsCreated { get; private set; }

        public IPlayer Value { get; private set; }

        public LazyPlayer(IPlayerFactory playerFactory)
        {
            playerFactory.WhenPlayerAvailable(player =>
            {
                IsCreated = true;
                Value = player;
            });
        }
    }
}