namespace Core.Characters
{
    public class EnemyAliveStateControl
    {
        private readonly EnemyMarker[] _enemies;

        public EnemyAliveStateControl(IObjectLocator objectLocator, IGameLoader gameLoader)
        {
            _enemies = objectLocator.FindAll<EnemyMarker>();
        }
    }
}