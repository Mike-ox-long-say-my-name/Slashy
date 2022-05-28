using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    public class AttackAnimationEvents : MonoBehaviour
    {
        public event Action DisableHitbox;
        public event Action EnableHitbox;
        public event Action AnimationEnded;

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
            AnimationEnded?.Invoke();
        }
    }
}