using Core;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour, IMovement
    {
        [SerializeField, Min(0)] private float jumpStartVelocity = 5;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float groundedGravity = -0.5f;
        [SerializeField] private float horizontalSpeed = 5f;
        [SerializeField] private float verticalSpeed = 5f;
        [SerializeField, Range(0, 1)] private float airboneControlFactor;
        [SerializeField] private float maxVelocity;
        [SerializeField] private float minVelocity;

        [SerializeField] private CharacterController characterController;

        private float _horizontalAirboneVelocity, _verticalAirboneVelocity;
        private Vector3 _velocity;

        public Vector3 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        public bool IsGrounded => characterController.isGrounded;

        private void Awake()
        {
            if (characterController == null)
            {
                Debug.LogWarning("Character Controller is not assigned", this);
                enabled = false;
            }
        }

        public void ApplyGravity()
        {
            _velocity.y += (characterController.isGrounded ? groundedGravity : gravity) * Time.deltaTime;
        }

        public void ApplyAirboneVelocity(Vector2 input)
        {
            _velocity.x = Mathf.SmoothDamp(_velocity.x, horizontalSpeed * input.x,
                ref _horizontalAirboneVelocity, airboneControlFactor);
            _velocity.z = Mathf.SmoothDamp(_velocity.z, verticalSpeed * input.y,
                ref _verticalAirboneVelocity, airboneControlFactor);
        }

        private void ClampVelocity()
        {
            _velocity.x = Mathf.Clamp(_velocity.x, minVelocity, maxVelocity);
            _velocity.y = Mathf.Clamp(_velocity.y, minVelocity, maxVelocity);
            _velocity.z = Mathf.Clamp(_velocity.z, minVelocity, maxVelocity);
        }

        public void Move(Vector2 move)
        {
            _velocity.x = horizontalSpeed * move.x;
            _velocity.z = verticalSpeed * move.y;
        }

        public void Jump()
        {
            _velocity.y = jumpStartVelocity;
        }

        public void ResetXZVelocity()
        {
            _velocity.x = 0;
            _velocity.z = 0;
        }

        public void MoveRaw(Vector3 move)
        {
            characterController.Move(move);
        }

        private void Update()
        {
            ClampVelocity();
            MoveRaw(_velocity * Time.deltaTime);
        }
    }
}