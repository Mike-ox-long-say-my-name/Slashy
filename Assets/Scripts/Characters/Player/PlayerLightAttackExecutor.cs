using Attacking;
using Core;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerLightAttackExecutor : StandaloneAnimationAttackExecutor
    {
        [SerializeField, Min(0)] private float moveDistance = 0.4f;
        [SerializeField] private PlayerMovement movement;

        private void Awake()
        {
            if (movement == null)
            {
                Debug.LogWarning("BasePlayerData Movement is not assigned", this);
                enabled = false;
            }
        }

        protected override void OnShouldEnableHitbox(IHitSource source)
        {
            base.OnShouldEnableHitbox(source);

            var direction = movement.transform.right;
            movement.MoveRaw(direction * moveDistance);
        }

        protected override void OnShouldDisableHitbox(IHitSource source)
        {
            base.OnShouldDisableHitbox(source);

            movement.ResetXZVelocity();
        }
    }
}