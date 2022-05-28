using System;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.Levels;
using UnityEngine;

namespace Core.Player.Interfaces
{
    public interface IPlayer
    {
        ICharacter Character { get; }
        IResource Stamina { get; }
        IVelocityMovement VelocityMovement { get; }
        IHurtbox Hurtbox { get; }
        Transform Transform { get; }
        GameObject Object { get; }

        void Freeze();
        void Unfreeze();
        event Action TouchedBonfire;
        event Action<Vector3> StartedWarping;
        
        void StartWarp(Vector3 target);
        void EndWarp(Vector3 target);
    }
}