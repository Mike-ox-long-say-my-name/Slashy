namespace Core.Characters
{
    public class CharacterInterruption
    {
        public CharacterInterruptionType Type { get; }
        public HittableEntity Source { get; }

        public CharacterInterruption(CharacterInterruptionType type, HittableEntity source)
        {
            Type = type;
            Source = source;
        }
    }
}
