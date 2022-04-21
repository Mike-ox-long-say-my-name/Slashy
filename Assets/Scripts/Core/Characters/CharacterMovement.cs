using UnityEngine;

namespace Core.Characters
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float groundedGravity = -0.5f;
        [SerializeField] private float horizontalSpeed = 5f;
        [SerializeField] private float verticalSpeed = 5f;
        [SerializeField] private float maxVelocity = 20;
        [SerializeField] private float minVelocity = -20;

        [SerializeField] private CharacterController characterController;
        
        private Vector3 _velocity;

        public Vector3 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        public bool IsGrounded => characterController.isGrounded;

        public float HorizontalSpeed => horizontalSpeed;

        public float VerticalSpeed => verticalSpeed;

        protected virtual void Awake()
        {
            if (characterController == null)
            {
                Debug.LogWarning("Character Controller is not assigned", this);
                enabled = false;
            }
        }

        public virtual void ApplyGravity()
        {
            _velocity.y += (IsGrounded ? groundedGravity : gravity) * Time.deltaTime;
        }

        private void ClampVelocity()
        {
            _velocity.x = Mathf.Clamp(_velocity.x, minVelocity, maxVelocity);
            _velocity.y = Mathf.Clamp(_velocity.y, minVelocity, maxVelocity);
            _velocity.z = Mathf.Clamp(_velocity.z, minVelocity, maxVelocity);
        }

        public virtual void Rotate(float direction)
        {
            if (Mathf.Abs(direction) > 0)
            {
                transform.eulerAngles = new Vector3(0, direction > 0 ? 0 : 180, 0);
            }
        }

        public virtual void Move(Vector2 input)
        {
            _velocity.x = horizontalSpeed * input.x;
            _velocity.z = verticalSpeed * input.y;
            Rotate(input.x);
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

        protected virtual void Update()
        {
            ClampVelocity();
            MoveRaw(_velocity * Time.deltaTime);
        }
    }
}