using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Player;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-10)]
    public class BorderManager : PublicSingleton<BorderManager>
    {
        private Border[] _borders;
        private int _aggroCounter = 0;

        protected override void Awake()
        {
            base.Awake();

            _borders = FindObjectsOfType<Border>();
            _borders = _borders
                .OrderBy(border => border.X)
                .ToArray();
        }

        public float GetAvailableCameraX(float currentX, float targetX, float minDistance)
        {
            var (left, right) = GetSurroundingBorders(currentX, true);
            if (left == null || right == null)
            {
                return targetX;
            }
            return Mathf.Clamp(targetX, left.X + minDistance, right.X - minDistance);
        }

        public void IncreaseAggroCounter()
        {
            if (_aggroCounter++ != 0)
            {
                return;
            }
            var player = PlayerManager.Instance.PlayerInfo.Transform;
            EnableSurroundingBorders(player.position.x);
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

        public void DecreaseAggroCounter()
        {
            if (--_aggroCounter == 0)
            {
                DisableAggroBorders();
            }
        }

        private void DisableAggroBorders()
        {
            foreach (var border in _borders.Where(border => border.IsAggroBorder))
            {
                border.Disable();
            }
        }

        public void ResetAggroCounter()
        {
            _aggroCounter = 0;
            DisableAggroBorders();
        }
    }
}
