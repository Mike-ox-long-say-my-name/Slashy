namespace Core.Characters.Interfaces
{
    public interface ICharacterResource
    {
        ICharacter Character { get; }

        float MaxValue { get; }
        float Value { get; }
    }
}