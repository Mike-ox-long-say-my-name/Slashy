namespace Core.Characters
{
    public interface ICharacterMovementConfig
    {
        float Gravity { get; }
        float GroundedGravity { get; }
        float HorizontalSpeed { get; }
        float VerticalSpeed { get; }
        float MaxVelocity { get; }
        float MinVelocity { get; }
    }
}