using UnityEngine;

namespace Core.Player.Interfaces
{
    public interface IAutoPlayerInput
    {
        void ResetBufferedInput();

        bool IsJumpPressed { get; }
        bool IsDashPressed { get; }
        bool IsLightAttackPressed { get; }
        bool IsHealPressed { get; }

        Vector2 MoveInput { get; }
    }
}