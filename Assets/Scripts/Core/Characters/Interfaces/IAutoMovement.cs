using System;
using Core.Modules;
using UnityEngine;

namespace Core.Characters.Interfaces
{
    public interface IAutoMovement : IUpdateable
    {
        void LockRotationOn(Transform lockOn);
        void UnlockRotation();

        void MoveTo(Vector3 position);
        void MoveTo(Transform target);
        void MoveToWithOffset(Transform target, Vector3 offset);
        void ResetTarget();

        void SetSpeedMultiplier(float speedMultiplier);
        void ResetSpeedMultiplier();
        void SetTargetReachedEpsilon(float epsilon);
        void ResetTargetReachedEpsilon();
        void SetMaxMoveTime(float time);
        void ResetMaxMoveTime();

        event Action TargetReached;
        void ResetTargetReached();
    }
}