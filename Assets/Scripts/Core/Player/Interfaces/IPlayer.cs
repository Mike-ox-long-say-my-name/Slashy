using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Player.Interfaces
{
    public interface IPlayer
    {
        IPlayerCharacter Player { get; }

        IVelocityMovement VelocityMovement { get; }
        IAutoPlayerInput Input { get; }

        IHurtbox Hurtbox { get; }
        
        bool IsJumping { get; }
        bool IsFalling { get; }
        bool IsDashing { get; }
        bool IsInvincible { get; }
        bool IsStaggered { get; }
        bool IsAttacking { get; }
        Transform Transform { get; }
    }
}