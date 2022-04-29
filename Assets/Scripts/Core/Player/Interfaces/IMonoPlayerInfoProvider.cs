namespace Core.Player.Interfaces
{
    public interface IMonoPlayerInfoProvider
    {
        IPlayerCharacter Player { get; }
        
        bool IsJumping { get; }
        bool IsFalling { get; }
        bool IsDashing { get; }
        bool IsInvincible { get; }
        bool IsStaggered { get; }
        bool IsAttacking { get; }
    }
}