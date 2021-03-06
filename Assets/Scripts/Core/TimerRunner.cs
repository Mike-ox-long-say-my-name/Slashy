using System;
using System.Collections.Generic;
using Core.Utilities;
using UnityEngine;

namespace Core
{
    public class TimerRunner : MonoBehaviour, ITimerRunner
    {
        private readonly List<Timer> _timers = new List<Timer>();
        
        public Timer CreateTimer(Action timeout = null, bool repeating = false)
        {
            var timer = new Timer()
            {
                IsRepeating = repeating
            };
            if (timeout != null)
            {
                timer.Timeout += timeout;
            }
            
            _timers.Add(timer);
            return timer;
        }

        public bool RemoveTimer(Timer timer) => _timers.Remove(timer);

        private void Update()
        {
            TickTimers(Time.deltaTime);
        }

        private void TickTimers(float deltaTime)
        {
            foreach (var timer in _timers)
            {
                timer.Tick(deltaTime);
            }
        }
    }
}