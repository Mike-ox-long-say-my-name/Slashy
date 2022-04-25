namespace Core.Characters.Interfaces
{
    public interface IPlayerMovementConfig : ICharacterMovementConfig
    {
        float JumpStartVelocity { get; }
        float AirboneControlFactor { get; }
    }

}