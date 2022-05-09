namespace Core.Characters.Interfaces
{
    public static class VelocityMovementExtensions
    {
        public static void Stop(this IVelocityMovement movement)
        {
            var velocity = movement.Velocity;
            velocity.x = 0;
            velocity.z = 0;
            movement.Velocity = velocity;
        }

        public static void ResetGravity(this IVelocityMovement movement)
        {
            var velocity = movement.Velocity;
            velocity.y = 0;
            movement.Velocity = velocity;
        }
    }
}