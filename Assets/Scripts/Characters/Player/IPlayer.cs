using Core.Characters;

namespace Characters.Player
{
    public interface IPlayer
    {
        IPlayerCharacter Character { get; }
        IPlayerMovement Movement { get; }

        bool IsInvincible { get; set; }
        bool IsJumping { get; }
        bool IsDashing { get; }
        bool IsFalling { get; }
        bool IsGroundState { get; }
        bool IsAttackState { get; }
        bool IsStaggered { get; }
    }
}