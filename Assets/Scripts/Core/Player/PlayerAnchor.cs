using UnityEngine;

namespace Core.Player
{
    public class PlayerAnchor : MonoBehaviour
    {
        private LazyPlayer _lazyPlayer;

        private void Awake()
        {
            var playerFactory = Container.Get<IPlayerFactory>();
            _lazyPlayer = playerFactory.GetLazyPlayer();
        }

        private void LateUpdate()
        {
            if (_lazyPlayer.IsCreated)
            {
                transform.position = _lazyPlayer.Value.Transform.position;
            }
        }
    }
}