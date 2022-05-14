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

        Animator Animator { get; }
        IHurtbox Hurtbox { get; }

        bool IsFrozen { get; set; }
        Transform Transform { get; }

        MixinPlayerCapabilities Capabilities { get; }

        GameObject PlayerObject { get; }
    }
}