using UnityEngine;

namespace Characters.Player
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