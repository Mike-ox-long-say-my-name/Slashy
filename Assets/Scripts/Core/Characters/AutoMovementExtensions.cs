using Core.Characters.Interfaces;

namespace Core.Characters
{
    public static class AutoMovementExtensions
    {
        public static void ResetState(this IAutoMovement movement)
        {
            movement.ResetSpeedMultiplier();
            movement.ResetTarget();
            movement.ResetTargetReachedEpsilon();
            movement.ResetMaxMoveTime();
            movement.ResetTargetReached();
        }
    }
}