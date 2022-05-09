using Core.Characters;
using Core.Characters.Interfaces;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Edit
{
    [TestFixture]
    public class MovementTests
    {
        private class MockBaseMovement : IBaseMovement
        {
            public bool IsGrounded { get; set; } = true;
            public Transform Transform { get; } = null;

            public Vector3 Position { get; set; }

            public void Move(Vector3 move)
            {
                Position += move;
            }

            public void Rotate(float direction)
            {
            }

            public void SetPosition(Vector3 position)
            {
            }
        }

        [Test]
        public void CharacterMovement_MovesWithVelocity()
        {
            var mock = new MockBaseMovement();
            var movement = new VelocityMovement(mock)
            {
                Gravity = -10,
                GroundedGravity = 0,
                HorizontalSpeed = 10,
                VerticalSpeed = 5,
                MaxVelocity = 20,
                MinVelocity = -20
            };
            movement.Move(new Vector3(1, 0, 1));
            movement.Tick(1);
            Assert.True(mock.Position == new Vector3(10, 0, 5));
        }

        [Test]
        public void CharacterMovement_Gravity()
        {
            var mock = new MockBaseMovement
            {
                IsGrounded = false
            };
            var movement = new VelocityMovement(mock)
            {
                Gravity = -10,
                GroundedGravity = -1,
                HorizontalSpeed = 10,
                VerticalSpeed = 5,
                MaxVelocity = 20,
                MinVelocity = -20
            };
            movement.Tick(2f);
            Assert.True(mock.Position == new Vector3(0, -40, 0));
        }

        [Test]
        public void CharacterMovement_GroundedGravity()
        {
            var mock = new MockBaseMovement
            {
                IsGrounded = true
            };
            var movement = new VelocityMovement(mock)
            {
                Gravity = -10,
                GroundedGravity = -0.05f,
                HorizontalSpeed = 10,
                VerticalSpeed = 5,
                MaxVelocity = 20,
                MinVelocity = -20
            };
            movement.Tick(10);
            Assert.True(mock.Position == new Vector3(0, -5, 0));
        }

        [Test]
        public void CharacterMovement_ClampsVelocity()
        {
            var mock = new MockBaseMovement();
            var movement = new VelocityMovement(mock)
            {
                MinVelocity = -20,
                MaxVelocity = 20,
                Gravity = -10
            };
            movement.Tick(10000);
            mock.Position = Vector3.zero;
            movement.Tick(10);
            Assert.True(mock.Position == new Vector3(0, -200, 0));
        }

        [Test]
        public void CharacterMovement_Stops()
        {
            var mock = new MockBaseMovement();
            var movement = new VelocityMovement(mock)
            {
                GroundedGravity = 0,
                HorizontalSpeed = 10,
                VerticalSpeed = 5,
                MaxVelocity = 20,
                MinVelocity = -20
            };
            movement.Move(Vector3.right);
            movement.Tick(1);
            movement.Velocity = Vector3.zero;
            movement.Tick(1);
            Assert.True(mock.Position == new Vector3(10, 0, 0));
        }
    }
}