namespace Core.Characters.Interfaces
{
    public interface ICharacterStats
    {
        public float MaxHealth { get; }
        public float MaxBalance { get; }
        public bool FreezeHealth { get; }
        public bool CanDie { get; }
    }
}