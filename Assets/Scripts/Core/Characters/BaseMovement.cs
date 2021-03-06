using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Characters
{
    public class BaseMovement : IBaseMovement
    {
        private readonly CharacterController _controller;
        public Transform Transform { get; }

        public BaseMovement(CharacterController controller)
        {
            Guard.NotNull(controller);

            _controller = controller;
            Transform = controller.transform;
        }

        public bool IsGrounded => _controller.isGrounded;

        public void Move(Vector3 move)
        {
            MoveWithFlags(move);
        }

        public CollisionFlags MoveWithFlags(Vector3 move)
        {
            return _controller.Move(move);
        }

        public void Rotate(float direction)
        {
            if (Mathf.Abs(direction) > 0)
            {
                Transform.eulerAngles = new Vector3(0, direction < 0 ? 180 : 0, 0);
            }
        }

        public void SetPosition(Vector3 position)
        {
            _controller.enabled = false;
            Transform.position = position;
            _controller.enabled = true;
        }
    }
}