using UnityEngine;

namespace Characters.Player
{
    public abstract class BasePlayerData : MonoBehaviour
    {
        public abstract PlayerCharacter PlayerCharacter { get; }
        public abstract PlayerMovement Movement { get; }

        public abstract bool IsInvincible { get; set; }
        public abstract bool IsJumping { get; }
        public abstract bool IsDashing { get; }
        public abstract bool IsFalling { get; }
        public abstract bool IsGroundState { get; }
        public abstract bool IsAttackState { get; }
        public abstract bool IsStaggered { get; }
    }
}