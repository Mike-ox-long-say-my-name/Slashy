using Characters.Player;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Player;
using Core.Utilities;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(RawMovementContainer))]
    public class PlayerMovementContainer : MonoBehaviour, IMovementExtension
    {
        [SerializeField] private MonoVelocityMovementConfig velocityMovementConfig;
        [SerializeField] private MonoPlayerMovementConfig playerMovementConfig;

        private IPlayerMovement _movement;

        IVelocityMovement IMovementExtension.Movement => Movement;

        public IPlayerMovement Movement
        {
            get
            {
                if (_movement != null)
                {
                    return _movement;
                }

                var rawMovement = GetComponent<RawMovementContainer>().Movement;
                _movement = new PlayerMovement(rawMovement, velocityMovementConfig.Config, playerMovementConfig.Config);
                return _movement;
            }
        }

        private void Awake()
        {
            Guard.NotNull(velocityMovementConfig);
            Guard.NotNull(playerMovementConfig);
        }
    }
}