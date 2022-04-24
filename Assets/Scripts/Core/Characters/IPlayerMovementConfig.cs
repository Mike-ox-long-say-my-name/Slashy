namespace Core.Characters
{
    public interface IPlayerMovementConfig : ICharacterMovementConfig
    {
        float JumpStartVelocity { get; }
        float AirboneControlFactor { get; }
    }

}