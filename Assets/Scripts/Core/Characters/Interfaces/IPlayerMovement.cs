namespace Core.Characters.Interfaces
{
    public interface IPlayerMovement : ICharacterMovement
    {
        IPushable Pushable { get; }

        void Jump();
    }
}