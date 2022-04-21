using Attacking;
using Core;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerLightAttackExecutor : StandaloneAnimationAttackExecutor
    {
        [SerializeField, Min(0)] private float moveDistance = 0.4f;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private CustomPlayerInput customPlayerInput;

        private bool _hasInput = true;

        private void Awake()
        {
            if (playerMovement == null)
            {
                Debug.LogWarning("Player Movement is not assigned", this);
                enabled = false;
            }

            if (customPlayerInput == null)
            {
                Debug.LogWarning("Player Input is not assigned", this);
                _hasInput = false;
            }
        }

        protected override void OnShouldEnableHitbox(IHitSource source)
        {
            base.OnShouldEnableHitbox(source);
            
            if (_hasInput)
            {
                var inputX = customPlayerInput.MoveInput.x;
                playerMovement.Rotate(inputX);
            }

            var direction = playerMovement.transform.right;
            playerMovement.MoveRaw(direction * moveDistance);
        }

        protected override void OnShouldDisableHitbox(IHitSource source)
        {
            base.OnShouldDisableHitbox(source);

            playerMovement.ResetXZVelocity();
        }
    }
}