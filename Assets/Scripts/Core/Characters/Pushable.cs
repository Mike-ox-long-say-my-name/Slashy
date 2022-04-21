using Core.Utilities;
using UnityEngine;

namespace Core.Characters
{
    [RequireComponent(typeof(CharacterMovement))]
    public class Pushable : MonoBehaviour
    {
        [Range(0, 2)] public float pushFactor = 1;
        [Min(0)] public float dampening = 0.2f;
        [Range(0, 1)] public float pushTime = 0.3f;

        private CharacterMovement _movement;
        private Vector3 _pushVelocity;

        private readonly TimedTrigger _pushing = new TimedTrigger();

        private void Awake()
        {
            _movement = GetComponent<CharacterMovement>();
        }
        
        public void Push(Vector3 direction, float force)
        {
            Push(direction, force, pushTime);
        }

        public void Push(Vector3 direction, float force, float time)
        {
            _pushVelocity = direction * force * pushFactor;
            _pushing.SetFor(time);
        }

        private void Update()
        {
            if (_pushing.IsFree)
            {
                return;
            }

            ApplyVelocity();
            ApplyDampening();

            _pushing.Step(Time.deltaTime);
        }

        private void ApplyDampening()
        {
            _pushVelocity *= 1 - dampening * Time.deltaTime;
        }

        private void ApplyVelocity()
        {
            var wasGrounded = _movement.IsGrounded;
            _movement.MoveRaw(_pushVelocity * Time.deltaTime);

            if (wasGrounded)
            {
                _movement.ApplyGravity();
            }
        }
    }
}