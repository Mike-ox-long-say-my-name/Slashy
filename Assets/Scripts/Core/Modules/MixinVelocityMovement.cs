using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Core.Modules
{
    [RequireComponent(typeof(MixinMovementBase))]
    public class MixinVelocityMovement : MonoBehaviour
    {
        [SerializeField] private MonoVelocityMovementConfig velocityMovementConfig;

        private IVelocityMovement _movement;

        public IVelocityMovement VelocityMovement
        {
            get
            {
                if (_movement != null)
                {
                    return _movement;
                }

                var movement = GetComponent<MixinMovementBase>().BaseMovement;
                _movement = new VelocityMovement(movement)
                {
                    AirboneControlFactor = velocityMovementConfig.AirboneControlFactor,
                    Gravity = velocityMovementConfig.Gravity,
                    GroundedGravity = velocityMovementConfig.GroundedGravity,
                    HorizontalSpeed = velocityMovementConfig.HorizontalSpeed,
                    VerticalSpeed = velocityMovementConfig.VerticalSpeed,
                    MaxVelocity = velocityMovementConfig.MaxVelocity,
                    MinVelocity = velocityMovementConfig.MinVelocity
                };
                return _movement;
            }
        }
    }
}