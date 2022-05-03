using Core.Attacking.Interfaces;

namespace Core.Player.Interfaces
{
    public interface IPlayer
    {
        IPlayerCharacter Player { get; }
        IHurtbox Hurtbox { get; }
        
        bool IsJumping { get; }
        bool IsFalling { get; }
        bool IsDashing { get; }
        bool IsInvincible { get; }
        bool IsStaggered { get; }
        bool IsAttacking { get; }
    }
}