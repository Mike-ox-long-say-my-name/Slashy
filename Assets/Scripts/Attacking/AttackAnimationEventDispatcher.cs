using UnityEngine;
using UnityEngine.Events;

namespace Attacking
{
    public class AttackAnimationEventDispatcher : MonoBehaviour
    {
        public UnityEvent AnimationShouldEnableHitbox { get; private set; } = new UnityEvent();
        public UnityEvent AnimationShouldDisableHitbox { get; private set; } = new UnityEvent();
        public UnityEvent AnimationShouldEndAttack { get; private set; } = new UnityEvent();

        public void OnAnimationShouldEnableHitbox()
        {
            AnimationShouldEnableHitbox.Invoke();
            AnimationShouldEnableHitbox = new UnityEvent();
        }

        public void OnAnimationShouldDisableHitbox()
        {
            AnimationShouldDisableHitbox.Invoke();
            AnimationShouldDisableHitbox = new UnityEvent();
        }

        public void OnAnimationShouldEndAttack()
        {
            AnimationShouldEndAttack.Invoke();
            AnimationShouldEndAttack = new UnityEvent();
        }
    }

}