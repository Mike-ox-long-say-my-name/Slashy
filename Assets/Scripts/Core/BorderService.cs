using System.Linq;
using UnityEngine;

namespace Core
{
    public class BorderService : IBorderService
    {
        private readonly Border[] _borders;

        private readonly LazyPlayer _player;

        public BorderService(IObjectLocator objectLocator, IAggroListener aggroListener, IPlayerFactory playerFactory)
        {
            _borders = objectLocator.FindAll<Border>().OrderBy(border => border.PositionX).ToArray();
            
            _player = playerFactory.GetLazyPlayer();

            aggroListener.FightStarted += EnableAggroBorders;
            aggroListener.FightEnded += DisableAggroBorders;
        }

        private void EnableAggroBorders()
        {
            if (_player.IsCreated)
            {
                EnableSurroundingBorders(_player.Value.Transform.position.x);
            }
        }

        public float GetAvailableCameraX(Camera cam, float targetX, float minDistance)
        {
            var (left, right) = GetSurroundingBorders(cam.transform.position.x, true);
            if (left == null || right == null)
            {
                return targetX;
            }

            const float referenceAspect = 16f / 9f;
            minDistance *= cam.aspect / referenceAspect;
            return Mathf.Clamp(targetX, left.PositionX + minDistance, right.PositionX - minDistance);
        }

        private void EnableSurroundingBorders(float x)
        {
            var (left, right) = GetSurroundingBorders(x);
            if (left == null || right == null)
            {
                return;
            }

            left.Enable();
            right.Enable();
        }

        private (Border left, Border right) GetSurroundingBorders(float x, bool onlyEnabled = false)
        {
            var searchThrough = onlyEnabled ? _borders.Where(border => border.IsEnabled).ToArray() : _borders;

            for (var i = 0; i < searchThrough.Length - 1; i++)
            {
                var left = searchThrough[i];
                var right = searchThrough[i + 1];

                if (left == null || right == null)
                {
                    Debug.LogWarning("Border was null");
                    continue;
                }

                if (!(left.PositionX <= x) || !(right.PositionX >= x))
                {
                    continue;
                }

                return (left, right);
            }

            return (null, null);
        }

        private void DisableAggroBorders()
        {
            foreach (var border in _borders.Where(border => border.IsAggroBorder))
            {
                border.Disable();
            }
        }
    }
}