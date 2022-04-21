namespace Core.Characters
{
    public interface ICharacterResource
    {
        Character Character { get; }

        float MaxValue { get; }
        float Value { get; set; }
    }
}