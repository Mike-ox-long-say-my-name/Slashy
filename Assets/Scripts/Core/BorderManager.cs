using System;
using Core.Player;
using System.Linq;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-10)]
    public class BorderManager : PublicSingleton<BorderManager>
    {
        private Border[] _borders;

        protected override void Awake()
        {
            base.Awake();

            _borders = FindObjectsOfType<Border>();
            _borders = _borders
                .OrderBy(border => border.X)
                .ToArray();
        }

        private void Start()
        {
            var fightManager = FightManager.Instance;
            fightManager.FightStarted += EnableAggroBorders;
            fightManager.FightEnded += DisableAggroBorders;
        }

        private void EnableAggroBorders()
        {
            var player = PlayerManager.Instance.PlayerInfo.Transform;
            EnableSurroundingBorders(player.position.x);
        }

        private void OnDestroy()
        {
            var fightManager = FightManager.TryGetInstance();
            if (fightManager == null)
            {
                return;
            }
            fightManager.FightStarted -= EnableAggroBorders;
            fightManager.FightEnded -= DisableAggroBorders;
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
            return Mathf.Clamp(targetX, left.X + minDistance, right.X - minDistance);
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

        public (Border left, Border right) GetSurroundingBorders(float x, bool onlyEnabled = false)
        {
            var searchThrough = onlyEnabled ? _borders.Where(border => border.IsEnabled).ToArray() : _borders;

            for (int i = 0; i < searchThrough.Length - 1; i++)
            {
                var left = searchThrough[i];
                var right = searchThrough[i + 1];

                if (left == null || right == null)
                {
                    Debug.LogWarning("Border was null");
                    continue;
                }

                if (!(left.X <= x) || !(right.X >= x))
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
