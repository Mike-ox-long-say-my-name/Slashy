using UnityEngine;

namespace Core.Characters
{
    public class Pushable : MonoBehaviour
    {
        [SerializeField] private CharacterMovement movement;
        [SerializeField, Range(0, 1)] private float dampening = 0.2f;
        [SerializeField, Min(0)] private float minPushDistance = 0.03f;

        private Vector3 _pushVelocity;

        private void Awake()
        {
            if (movement == null)
            {
                Debug.LogWarning("Character Movement is not assigned", this);
                enabled = false;
            }
        }

        public void Push(Vector3 force, bool overrideVelocity = false)
        {
            _pushVelocity = overrideVelocity ? force : _pushVelocity + force;
        }

        private void Update()
        {
            if (_pushVelocity.magnitude < minPushDistance)
            {
                _pushVelocity = Vector3.zero;
            }
            else
            {
                ApplyVelocity();
                ApplyDampening();
            }
        }

        private void ApplyDampening()
        {
            _pushVelocity *= 1 - dampening;
        }

        private void ApplyVelocity()
        {
            movement.MoveRaw(_pushVelocity * Time.deltaTime);
        }
    }
}