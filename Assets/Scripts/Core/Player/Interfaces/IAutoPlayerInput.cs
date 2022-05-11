using System;
using UnityEngine;

namespace Core.Player.Interfaces
{
    public interface IAutoPlayerInput
    {
        void ResetBufferedInput();

        bool IsJumpPressed { get; }
        bool IsDashPressed { get; }
        bool IsLightAttackPressed { get; }
        bool IsStrongAttackPressed { get; }
        bool IsHealPressed { get; }

        event Action Interacted;

        Vector2 MoveInput { get; }
    }
}