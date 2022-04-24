namespace Core.Characters
{
    public interface IPlayerMovement : ICharacterMovement
    {
        IPushable Pushable { get; }

        void Jump();
    }
}