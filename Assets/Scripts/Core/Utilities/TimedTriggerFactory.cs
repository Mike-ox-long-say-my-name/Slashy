using System.Collections.Generic;
using UnityEngine;

namespace Core.Utilities
{
    public sealed class TimedTriggerFactory
    {
        private readonly List<TimedTrigger> _triggers = new List<TimedTrigger>();

        public IEnumerable<TimedTrigger> Triggers => _triggers;

        public TimedTrigger Create()
        {
            var trigger = new TimedTrigger();
            _triggers.Add(trigger);
            return trigger;
        }

        public void Delete(TimedTrigger trigger)
        {
            _triggers.Remove(trigger);
        }

        public void StepAll()
        {
            StepAll(Time.deltaTime);
        }

        public void StepAll(float timeStep)
        {
            foreach (var trigger in _triggers)
            {
                trigger.Step(timeStep);
            }
        }

        public void ResetAll()
        {
            foreach (var trigger in _triggers)
            {
                trigger.Reset();
            }
        }
    }
}