using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    public class MixinAttackAnimationEventDispatcher : MonoBehaviour
    {
        [SerializeField] private UnityEvent disableHitbox;
        [SerializeField] private UnityEvent enableHitbox;
        [SerializeField] private UnityEvent endAttack;

        public UnityEvent DisableHitbox => disableHitbox;
        public UnityEvent EnableHitbox => enableHitbox;
        public UnityEvent EndAttack => endAttack;

        [UsedImplicitly]
        public void OnAnimationShouldEnableHitbox()
        {
            EnableHitbox?.Invoke();
        }

        [UsedImplicitly]
        public void OnAnimationShouldDisableHitbox()
        {
            DisableHitbox?.Invoke();
        }

        [UsedImplicitly]
        public void OnAnimationShouldEndAttack()
        {
            EndAttack?.Invoke();
        }
    }
}