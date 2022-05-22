using System;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-10)]
    public class FightManager : PublicSingleton<FightManager>
    {
        public event Action FightStarted;
        public event Action FightEnded;

        private int _aggroCounter = 0;

        public bool IsFighting => _aggroCounter > 0;

        public void IncreaseAggroCounter()
        {
            _aggroCounter++;
            if (_aggroCounter == 1)
            {
                FightStarted?.Invoke();
            }
        }

        public void DecreaseAggroCounter()
        {
            _aggroCounter--;
            if (_aggroCounter == 0)
            {
                FightEnded?.Invoke();
            }
        }

        public void ResetAggroCounter()
        {
            if (_aggroCounter == 0)
            {
                return;
            }

            _aggroCounter = 0;
            FightEnded?.Invoke();
        }
    }
}