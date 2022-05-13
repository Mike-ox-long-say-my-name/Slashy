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
        }

        public float GetAvailableCameraX()
        {
            throw new NotImplementedException();
        }

        public void IncreaseAggroCounter()
        {
            if (_aggroCounter++ != 0)
            {
                return;
            }
            var player = PlayerManager.Instance.PlayerInfo.Transform;
            EnableClosestBorders(player.position.x);
        }

        private void EnableClosestBorders(float x)
        {
            for (int i = 0; i < _borders.Length - 1; i++)
            {
                var left = _borders[i];
                var right = _borders[i + 1];

                if (!(left.X <= x) || !(right.X >= x))
                {
                    continue;
                }

                left.Enable();
                right.Enable();
                break;
            }
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
    }
}
