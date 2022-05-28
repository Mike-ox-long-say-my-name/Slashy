using Core.Levels;
using UnityEngine;

namespace Core
{
    public class LevelBootstrap : MonoBehaviour
    {
        [SerializeField] private UiType levelUiType = UiType.Default;

        private void Awake()
        {
            var uiFactory = Container.Get<IUiFactory>();
            var enemyFactory = Container.Get<IEnemyFactory>();
            var playerFactory = Container.Get<IPlayerFactory>();
            var levelWarper = Container.Get<ILevelWarper>();

            uiFactory.CreateUi(levelUiType);
            enemyFactory.CreateAllOnLevelAtEnemyMarkers();

            if (levelWarper.IsWarping)
            {
                playerFactory.CreatePlayer(levelWarper.CreationInfo);
            }
            else
            {
                playerFactory.CreatePlayerAtPlayerMarker();
            }
        }
    }
}