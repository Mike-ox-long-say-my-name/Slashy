using System;
using UnityEngine;

namespace Core.Utilities
{
    public class Timer
    {
        public bool IsRunning { get; private set; }
        public float TimeRemained { get; private set; }

        public event Action Timeout;

        public void Start(float time)
        {
            if (time < 0)
            {
                Invoke();
                return;
            }

            IsRunning = true;
            TimeRemained = time;
        }

        public void Step(float deltaTime)
        {
            if (!IsRunning)
            {
                return;
            }

            TimeRemained = Mathf.Max(0, TimeRemained - deltaTime);
            if (!Mathf.Approximately(TimeRemained, 0))
            {
                return;
            }
            IsRunning = false;
            Invoke();
        }

        private void Invoke()
        {
            Timeout?.Invoke();
        }

        public static Timer Start(float time, Action timeout)
        {
            var timer = new Timer();
            timer.Timeout += timeout;
            timer.Start(time);
            return timer;
        }
    }
}