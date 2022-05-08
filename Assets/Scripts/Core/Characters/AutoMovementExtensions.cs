namespace Core.Characters
{
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