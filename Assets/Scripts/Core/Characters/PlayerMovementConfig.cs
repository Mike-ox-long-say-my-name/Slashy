using UnityEngine;

namespace Core.Characters
{
    [CreateAssetMenu(menuName = "Player Movement Config", fileName = "PlayerMovementConfig", order = 0)]
    public class PlayerMovementConfig : CharacterMovementConfig, IPlayerMovementConfig
    {
        [SerializeField, Min(0)] private float jumpStartVelocity = 5;
        [SerializeField, Range(0, 1)] private float airboneControlFactor = 0.7f;

        public float JumpStartVelocity => jumpStartVelocity;
        public float AirboneControlFactor => airboneControlFactor;
    }
}