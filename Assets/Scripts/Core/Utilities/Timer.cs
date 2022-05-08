using System;
using UnityEngine;

namespace Core.Utilities
{
    public class Timer
    {
        public bool IsRunning { get; private set; }
        public float TimeRemained { get; private set; }

        private float _baseTime;
        public bool IsRepeating { get; private set; }

        public event Action Timeout;

        public void Start(float time, bool repeating = false)
        {
            if (time < 0)
            {
                Invoke();
                return;
            }

            IsRepeating = repeating;
            IsRunning = true;
            _baseTime = time;
            TimeRemained = time;
        }

        public void Tick(float deltaTime)
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

            Invoke();
            if (IsRepeating)
            {
                TimeRemained = _baseTime;
            }

            IsRunning = IsRepeating;
        }

        private void Invoke()
        {
            Timeout?.Invoke();
        }

        public static Timer Start(float time, Action timeout, bool repeating = false)
        {
            var timer = new Timer();
            timer.Timeout += timeout;
            timer.Start(time, repeating);
            return timer;
        }

        public void Stop()
        {
            TimeRemained = 0;
            _baseTime = 0;
            IsRunning = false;
        }
    }
}