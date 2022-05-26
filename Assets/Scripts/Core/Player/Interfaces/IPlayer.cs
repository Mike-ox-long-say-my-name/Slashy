using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Player.Interfaces
{
    public interface IPlayer
    {
        IPlayerCharacter PlayerCharacter { get; }
        IVelocityMovement VelocityMovement { get; }
        Animator Animator { get; }
        IHurtbox Hurtbox { get; }
        Transform Transform { get; }
        GameObject PlayerObject { get; }

        void Freeze();
        void Unfreeze();
    }
}