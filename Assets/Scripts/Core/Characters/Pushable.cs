using Core.Utilities;
using UnityEngine;

namespace Core.Characters
{
    public class Pushable : IPushable
    {
        public float PushFactor { get; set; } = 1;
        public float Dampening { get; set; } = 0.2f;
        public float PushTime { get; set; } = 0.3f;

        private readonly IRawMovement _movement;

        private readonly TimedTrigger _pushing = new TimedTrigger();
        private Vector3 _pushVelocity;

        public Pushable(IRawMovement movement)
        {
            _movement = movement;
        }

        public bool IsPushing => _pushing.IsSet;

        public void Push(Vector3 direction, float force)
        {
            Push(direction, force, PushTime);
        }

        public void Push(Vector3 direction, float force, float time)
        {
            _pushVelocity = direction * (force * PushFactor);
            _pushing.SetFor(time);
        }

        public void Tick(float deltaTime)
        {
            if (!IsPushing)
            {
                return;
            }

            ApplyVelocity();
            ApplyDampening();

            _pushing.Step(Time.deltaTime);
        }

        private void ApplyDampening()
        {
            _pushVelocity *= 1 - Dampening * Time.deltaTime;
        }

        private void ApplyVelocity()
        {
            _movement.MoveRaw(_pushVelocity * Time.deltaTime);
        }
    }
}