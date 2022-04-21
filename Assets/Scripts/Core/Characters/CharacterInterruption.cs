namespace Core.Characters
{
    public class CharacterInterruption
    {
        public CharacterInterruptionType Type { get; }
        public IHitSource Source { get; }

        public CharacterInterruption(CharacterInterruptionType type, IHitSource source)
        {
            Type = type;
            Source = source;
        }
    }
}
