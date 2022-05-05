using System;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters
{
    public interface IAutoMovement : IVelocityMovement
    {
        void LockRotationOn(Transform lockOn);
        void UnlockRotation();

        void MoveTo(Vector3 position);
        void MoveTo(Transform target);
        void ResetTarget();

        void SetSpeedMultiplier(float speedMultiplier);
        void ResetSpeedMultiplier();
        void SetTargetReachedEpsilon(float epsilon);
        void ResetTargetReachedEpsilon();

        event Action TargetReached;
    }

    public static class AutoMovementExtensions
    {
        public static void ResetState(this IAutoMovement movement)
        {
            movement.ResetSpeedMultiplier();
            movement.ResetTarget();
            movement.ResetTargetReachedEpsilon();
        }
    }
}