using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters.Mono
{
    [CreateAssetMenu(menuName = "Character Movement Config", fileName = "CharacterMovementConfig", order = 0)]
    public class CharacterMovementConfig : ScriptableObject, ICharacterMovementConfig
    {
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float groundedGravity = -0.5f;
        [SerializeField, Min(0)] private float horizontalSpeed = 5f;
        [SerializeField, Min(0)] private float verticalSpeed = 5f;
        [SerializeField] private float maxVelocity = 20;
        [SerializeField] private float minVelocity = -20;

        public float Gravity => gravity;

        public float GroundedGravity => groundedGravity;

        public float HorizontalSpeed => horizontalSpeed;

        public float VerticalSpeed => verticalSpeed;

        public float MaxVelocity => maxVelocity;

        public float MinVelocity => minVelocity;
    }
}