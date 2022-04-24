namespace Core.Characters
{
    public interface ICharacterStats
    {
        public float MaxHealth { get; }
        public float MaxBalance { get; }
        public bool FreezeHealth { get; }
        public bool CanDie { get; }
    }
}