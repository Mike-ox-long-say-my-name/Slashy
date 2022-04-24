namespace Core.Characters
{
    public interface ICharacterResource
    {
        ICharacter Character { get; }

        float MaxValue { get; }
        float Value { get; }
    }
}