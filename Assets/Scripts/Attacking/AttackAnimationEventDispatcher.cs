using System;
using UnityEngine;
using UnityEngine.Events;

namespace Attacking
{
    public class AttackAnimationEventDispatcher : MonoBehaviour
    {
        private Action _animationShouldEnableHitbox;
        private Action _animationShouldDisableHitbox;
        private Action _animationShouldEndAttack;

        public void SetAnimationShouldEnableHitbox(Action action)
        {
            _animationShouldEnableHitbox = action;
        }

        public void SetAnimationShouldDisableHitbox(Action action)
        {
            _animationShouldDisableHitbox = action;
        }

        public void SetAnimationShouldEndAttack(Action action)
        {
            _animationShouldEndAttack = action;
        }

        public void OnAnimationShouldEnableHitbox()
        {
            _animationShouldEnableHitbox?.Invoke();
            _animationShouldEnableHitbox = null;
        }

        public void OnAnimationShouldDisableHitbox()
        {
            _animationShouldDisableHitbox?.Invoke();
            _animationShouldDisableHitbox = null;
        }

        public void OnAnimationShouldEndAttack()
        {
            _animationShouldEndAttack?.Invoke();
            _animationShouldEndAttack = null;
        }
    }

}